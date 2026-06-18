import { Component, OnInit } from '@angular/core';
import { AbstractControl, UntypedFormArray, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { forkJoin, of } from 'rxjs';
import { catchError, first } from 'rxjs/operators';
import Swal from 'sweetalert2';
import { GlobalComponent } from 'src/app/global-component';
import { MyAuthService } from 'src/app/core/services/my-auth.service';
import { ToastService } from '../../icons/toast-service';
import { AspNetUser } from '../interfaces/aspnet-user.interface';
import { Brand } from '../interfaces/brand.interface';
import { Branch } from '../interfaces/branch.interface';
import { CarCarColor } from '../interfaces/car-car-color.interface';
import { CarModel } from '../interfaces/car-model.interface';
import { Car } from '../interfaces/car.interface';
import { CreateInvoice } from '../interfaces/invoice.interface';
import { CompanyInfo } from '../interfaces/company-info.interface';
import { LookupDetail } from '../interfaces/lookup.interface';
import { BrandService } from '../services/brand.service';
import { BranchService } from '../services/branch.service';
import { CarCarColorService } from '../services/car-car-color.service';
import { CarModelService } from '../services/car-model.service';
import { CarService } from '../services/car.service';
import { CompanyInfoService } from '../services/company-info.service';
import { InvoiceService } from '../services/invoice.service';
import { LookupService } from '../services/lookup.service';
import { AdminUserService } from '../services/admin-user.service';
import { getErrorMessage } from '../shared/error-message.util';

interface InvoicePreview {
  invoiceNumber: string;
  customerName: string;
  total: number;
  itemsCount: number;
  createdAt: string;
}

@Component({
  selector: 'app-invoice-create-page',
  templateUrl: './invoice-create-page.component.html',
  styleUrl: './invoice-create-page.component.scss',
  standalone: false
})
export class InvoiceCreatePageComponent implements OnInit {
  breadCrumbItems!: Array<{}>;
  invoiceForm!: UntypedFormGroup;
  isLoading = false;
  isSubmitting = false;
  submitted = false;
  lastCreatedInvoice?: InvoicePreview;
  currentUserBranchId: number | null = null;
  customerUsers: AspNetUser[] = [];
  filteredCustomerUsers: AspNetUser[] = [];
  customerUserSearchTerm = '';
  selectedCustomerUser?: AspNetUser;
  isCustomerModalOpen = false;
  isLoadingCustomerUsers = false;

  cars: Car[] = [];
  branches: Branch[] = [];
  brands: Brand[] = [];
  models: CarModel[] = [];
  paymentMethods: LookupDetail[] = [];
  companyInfo: CompanyInfo | null = null;

  private readonly colorsByCarId = new Map<number, CarCarColor[]>();
  private readonly imageByCarId = new Map<number, string | null>();
  private readonly fallbackCarImage = 'assets/images/car-placeholder.png';

  constructor(
    private formBuilder: UntypedFormBuilder,
    private carService: CarService,
    private branchService: BranchService,
    private brandService: BrandService,
    private carModelService: CarModelService,
    private carCarColorService: CarCarColorService,
    private companyInfoService: CompanyInfoService,
    private invoiceService: InvoiceService,
    private lookupService: LookupService,
    private adminUserService: AdminUserService,
    private myAuthService: MyAuthService,
    private router: Router,
    private toastService: ToastService,
    private translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.currentUserBranchId = Number(this.myAuthService.currentUserValue?.branchId) || null;

    this.breadCrumbItems = [
      { label: this.translate.instant('MENUITEMS.REQUEST.TEXT') },
      { label: this.translate.instant('MENUITEMS.REQUEST.LIST.CREATE_INVOICE'), active: true }
    ];

    this.invoiceForm = this.formBuilder.group({
      invoiceNumber: [this.generateInvoiceNumber(), Validators.required],
      issueDate: [this.getNowInputValue(), Validators.required],
      dueDate: [this.getFutureInputValue(7), Validators.required],
      branchId: [this.currentUserBranchId, Validators.required],
      paymentMethod: [null, Validators.required],
      userId: [''],
      customerName: ['', [Validators.required, Validators.maxLength(200)]],
      customerPhone: ['', [Validators.required, Validators.maxLength(20)]],
      customerEmail: ['', [Validators.email, Validators.maxLength(256)]],
      customerAddress: ['', [Validators.maxLength(500)]],
      notes: ['', [Validators.maxLength(1000)]],
      shippingFee: [0, [Validators.min(0)]],
      extraDiscount: [0, [Validators.min(0)]],
      items: this.formBuilder.array([])
    });

    this.addInvoiceItem();
    this.loadDependencies();
  }

  get form() {
    return this.invoiceForm.controls;
  }

  get items(): UntypedFormArray {
    return this.invoiceForm.get('items') as UntypedFormArray;
  }

  get activeItemsCount(): number {
    return this.items.controls.filter(control => Number(control.get('carId')?.value) > 0).length;
  }

  get subtotal(): number {
    return this.items.controls.reduce((total, control) => total + this.getLineBaseAmount(control), 0);
  }

  get lineDiscountTotal(): number {
    return this.items.controls.reduce((total, control) => total + this.getLineDiscount(control), 0);
  }

  get vatTotal(): number {
    return this.items.controls.reduce((total, control) => total + this.getLineVatAmount(control), 0);
  }

  get grandTotal(): number {
    const subtotal = this.subtotal;
    const lineDiscount = this.lineDiscountTotal;
    const extraDiscount = this.toNumber(this.form['extraDiscount'].value);
    const shipping = this.toNumber(this.form['shippingFee'].value);
    const total = subtotal - lineDiscount - extraDiscount + this.vatTotal + shipping;
    return total > 0 ? total : 0;
  }

  loadDependencies(): void {
    this.isLoading = true;

    forkJoin({
      cars: this.carService.getCars().pipe(first()),
      branches: this.branchService.getBranches().pipe(first()),
      brands: this.brandService.getBrands().pipe(first()),
      models: this.carModelService.getModels().pipe(first()),
      paymentMethods: this.lookupService.getByMasterCode('PAYMENT_METHOD').pipe(first()),
      companyInfos: this.companyInfoService.getCompanyInfos().pipe(
        catchError(() => of([] as CompanyInfo[])),
        first()
      )
    }).subscribe({
      next: ({ cars, branches, brands, models, paymentMethods, companyInfos }) => {
        this.cars = (cars || []).filter(car => car.isAvailable);
        this.branches = (branches || []).filter(branch => branch.isAvailable);
        this.brands = (brands || []).filter(brand => brand.isAvailable !== false);
        this.models = (models || []).filter(model => model.isAvailable);
        this.paymentMethods = paymentMethods || [];
        const companyList = companyInfos || [];
        this.companyInfo = companyList.find(item => item.isAvailable) || companyList[0] || null;
        this.applyDefaultBranch();
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.showError(error);
      }
    });
  }

  addInvoiceItem(): void {
    this.items.push(this.createInvoiceItemGroup());
  }

  removeInvoiceItem(index: number): void {
    if (this.items.length === 1) {
      this.items.at(0).reset({
        carId: null,
        colorId: null,
        quantity: 1,
        unitPrice: 0,
        lineDiscount: 0,
        lineNotes: ''
      });
      return;
    }

    this.items.removeAt(index);
  }

  onCarSelectionChange(index: number): void {
    const group = this.items.at(index) as UntypedFormGroup;
    const carId = Number(group.get('carId')?.value);

    group.patchValue(
      {
        colorId: null,
        unitPrice: 0,
        lineDiscount: 0
      },
      { emitEvent: false }
    );

    if (!carId) {
      return;
    }

    this.ensureCarResources(carId, () => {
      const colors = this.getAvailableColors(carId);
      if (colors.length > 0) {
        group.patchValue(
          {
            colorId: colors[0].colorId
          },
          { emitEvent: false }
        );
        this.applySuggestedPrice(index);
      }
    });
  }

  onColorSelectionChange(index: number): void {
    this.applySuggestedPrice(index);
  }

  async createInvoice(): Promise<void> {
    this.submitted = true;
    this.invoiceForm.markAllAsTouched();

    if (this.invoiceForm.invalid || this.items.length === 0 || this.activeItemsCount === 0) {
      return;
    }

    const confirmation = await Swal.fire({
      icon: 'question',
      title: this.translate.instant('INVOICE_PAGE.CONFIRM_CREATE_TITLE'),
      text: this.translate.instant('INVOICE_PAGE.CONFIRM_CREATE_TEXT'),
      showCancelButton: true,
      confirmButtonText: this.translate.instant('INVOICE_PAGE.CREATE_ACTION'),
      cancelButtonText: this.translate.instant('COMMON.CANCEL'),
      confirmButtonColor: '#0ab39c'
    });

    if (!confirmation.isConfirmed) {
      return;
    }

    this.isSubmitting = true;

    const payload: CreateInvoice = {
      userId: this.normalizeUserId(this.form['userId'].value),
      branchId: Number(this.form['branchId'].value),
      paymentMethod: Number(this.form['paymentMethod'].value),
      invoiceNumber: String(this.form['invoiceNumber'].value || '').trim(),
      issueDate: String(this.form['issueDate'].value || ''),
      dueDate: String(this.form['dueDate'].value || ''),
      customerName: String(this.form['customerName'].value || '').trim(),
      customerPhone: String(this.form['customerPhone'].value || '').trim(),
      customerEmail: this.normalizeString(this.form['customerEmail'].value),
      customerAddress: this.normalizeString(this.form['customerAddress'].value),
      notes: this.normalizeString(this.form['notes'].value),
      shippingFee: this.toNumber(this.form['shippingFee'].value),
      extraDiscount: this.toNumber(this.form['extraDiscount'].value),
      details: this.items.controls
        .filter(control => Number(control.get('carId')?.value) > 0)
        .map(control => ({
          carId: Number(control.get('carId')?.value),
          colorId: control.get('colorId')?.value ? Number(control.get('colorId')?.value) : null,
          quantity: this.toNumber(control.get('quantity')?.value, 1),
          unitPrice: this.toNumber(control.get('unitPrice')?.value),
          discountAmount: this.getLineDiscount(control),
          vatAmount: this.getLineVatAmount(control),
          notes: this.normalizeString(control.get('lineNotes')?.value)
        }))
    };

    this.invoiceService.create(payload).pipe(first()).subscribe({
      next: (createdInvoice) => {
        const createdInvoiceId = createdInvoice.id;
        this.lastCreatedInvoice = {
          invoiceNumber: createdInvoice.invoiceNumber,
          customerName: createdInvoice.customerName,
          total: createdInvoice.grandTotal,
          itemsCount: createdInvoice.details?.length || 0,
          createdAt: createdInvoice.createdAt
        };
        this.isSubmitting = false;
        this.toastService.show(this.translate.instant('INVOICE_PAGE.SUCCESS_MESSAGE'), {
          classname: 'bg-success text-white',
          delay: 3000
        });
        this.openInvoicePrintTab(createdInvoiceId, true);
        this.resetInvoice();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.showError(error);
      }
    });
  }

  resetInvoice(): void {
    this.submitted = false;
    this.lastCreatedInvoice = undefined;
    this.selectedCustomerUser = undefined;
    this.invoiceForm.reset({
      invoiceNumber: this.generateInvoiceNumber(),
      issueDate: this.getNowInputValue(),
      dueDate: this.getFutureInputValue(7),
      branchId: this.currentUserBranchId,
      paymentMethod: null,
      userId: '',
      customerName: '',
      customerPhone: '',
      customerEmail: '',
      customerAddress: '',
      notes: '',
      shippingFee: 0,
      extraDiscount: 0
    });

    while (this.items.length > 0) {
      this.items.removeAt(0);
    }

    this.addInvoiceItem();
  }

  openCustomerPicker(): void {
    this.isCustomerModalOpen = true;
    this.customerUserSearchTerm = '';

    if (this.customerUsers.length > 0) {
      this.applyCustomerUserFilters();
      return;
    }

    this.isLoadingCustomerUsers = true;
    this.adminUserService.getUsers().pipe(first()).subscribe({
      next: (users) => {
        this.customerUsers = (users || []).filter(user => !user.isLocked);
        this.applyCustomerUserFilters();
        this.isLoadingCustomerUsers = false;
      },
      error: (error) => {
        this.isLoadingCustomerUsers = false;
        this.showError(error);
      }
    });
  }

  closeCustomerPicker(): void {
    this.isCustomerModalOpen = false;
  }

  onCustomerUserSearch(): void {
    this.applyCustomerUserFilters();
  }

  selectCustomerUser(user: AspNetUser): void {
    this.selectedCustomerUser = user;
    this.invoiceForm.patchValue({
      customerName: this.getCustomerUserDisplayName(user),
      customerPhone: user.mobileNo || '',
      customerEmail: user.email || ''
    });
    this.closeCustomerPicker();
  }

  clearSelectedCustomerUser(): void {
    this.selectedCustomerUser = undefined;
    this.invoiceForm.patchValue({
      customerName: '',
      customerPhone: '',
      customerEmail: ''
    });
  }

  getCustomerUserDisplayName(user?: AspNetUser | null): string {
    if (!user) {
      return this.translate.instant('COMMON.NOT_AVAILABLE');
    }

    return String(
      user.nameAr ||
      user.fullNameAr ||
      user.nameEn ||
      user.fullNameEn ||
      user.userName ||
      user.email ||
      user.id
    ).trim();
  }

  getCustomerUserBranchName(user?: AspNetUser | null): string {
    if (!user?.branchId) {
      return this.translate.instant('COMMON.NOT_AVAILABLE');
    }

    const branch = this.branches.find(item => item.id === user.branchId);
    if (branch) {
      return this.getLocalizedValue(branch.branchNameAr, branch.branchNameEn, user.branchName || `#${user.branchId}`);
    }

    return user.branchName || `#${user.branchId}`;
  }

  getAvailableColors(carId: number | null | undefined): CarCarColor[] {
    if (!carId) {
      return [];
    }

    return this.colorsByCarId.get(carId) || [];
  }

  getCar(carId: number | null | undefined): Car | undefined {
    if (!carId) {
      return undefined;
    }

    return this.cars.find(car => car.id === carId);
  }

  getCarDisplayName(carId: number | null | undefined): string {
    const car = this.getCar(carId);
    if (!car) {
      return this.translate.instant('INVOICE_PAGE.UNKNOWN_CAR');
    }

    const localizedName = this.getLocalizedValue(car.nameAr, car.nameEn, '');
    if (localizedName) {
      return localizedName;
    }

    return `${this.getBrandNameByModelId(car.modelId)} ${this.getModelName(car.modelId)}`.trim() || `#${car.id}`;
  }

  getBrandNameByModelId(modelId: number): string {
    const model = this.models.find(item => item.id === modelId);
    const brand = this.brands.find(item => item.id === model?.brandId);
    return this.getLocalizedValue(brand?.nameAr, brand?.nameEn, this.translate.instant('COMMON.NOT_AVAILABLE'));
  }

  getModelName(modelId: number): string {
    const model = this.models.find(item => item.id === modelId);
    return this.getLocalizedValue(model?.nameAr, model?.nameEn, this.translate.instant('COMMON.NOT_AVAILABLE'));
  }

  getBranchName(branchId: number): string {
    const branch = this.branches.find(item => item.id === branchId);
    return this.getLocalizedValue(branch?.branchNameAr, branch?.branchNameEn, `#${branchId}`);
  }

  getPaymentMethodName(paymentMethodId: number | null | undefined): string {
    const method = this.paymentMethods.find(item => item.id === paymentMethodId);
    return this.getLookupDisplayName(method);
  }

  get sellerDisplayName(): string {
    return this.getLocalizedValue(this.companyInfo?.companyNameAr, this.companyInfo?.companyNameEn, '-');
  }

  get hasPreviewZatcaQrData(): boolean {
    return !!(
      this.companyInfo?.vatRegistrationNumber &&
      this.sellerDisplayName &&
      this.form['issueDate']?.value &&
      this.activeItemsCount > 0 &&
      this.grandTotal > 0
    );
  }

  get zatcaQrStatusMessage(): string {
    if (!this.companyInfo?.vatRegistrationNumber) {
      return this.translate.instant('INVOICE_PAGE.ZATCA_QR_MISSING_VAT');
    }

    return this.translate.instant('INVOICE_PAGE.ZATCA_QR_UNAVAILABLE');
  }

  getPreviewZatcaQrImageUrl(): string | null {
    const sellerName = this.sellerDisplayName.trim();
    const vatRegistrationNumber = this.companyInfo?.vatRegistrationNumber?.trim();
    const issueDate = String(this.form['issueDate']?.value || '').trim();

    if (!sellerName || !vatRegistrationNumber || !issueDate || this.grandTotal <= 0) {
      return null;
    }

    const params = new URLSearchParams({
      sellerName,
      vatRegistrationNumber,
      issueDate,
      invoiceTotalWithVat: this.grandTotal.toFixed(2),
      vatTotal: this.vatTotal.toFixed(2)
    });

    return `${GlobalComponent.API_URL}/api/Invoices/zatca-qr-preview.png?${params.toString()}`;
  }

  getLookupDisplayName(item?: LookupDetail | null): string {
    return this.getLocalizedValue(item?.nameAr, item?.nameEn, item?.displayName || this.translate.instant('COMMON.NOT_AVAILABLE'));
  }

  getLineTotal(control: AbstractControl): number {
    const total = this.getLineBaseAmount(control) - this.getLineDiscount(control) + this.getLineVatAmount(control);
    return total > 0 ? total : 0;
  }

  getColorDisplayName(item?: CarCarColor | null): string {
    return this.getLocalizedValue(item?.colorNameAr, item?.colorNameEn, this.translate.instant('COMMON.NOT_AVAILABLE'));
  }

  getColorPreview(carId: number | null | undefined, colorId: number | null | undefined): CarCarColor | undefined {
    if (!carId || !colorId) {
      return undefined;
    }

    return this.getAvailableColors(carId).find(item => item.colorId === colorId);
  }

  getCarImageUrl(carId: number | null | undefined): string {
    if (!carId) {
      return this.fallbackCarImage;
    }

    return this.resolveImageUrl(this.imageByCarId.get(carId) || undefined, this.fallbackCarImage);
  }

  getLocalizedValue(ar?: string | null, en?: string | null, fallback = ''): string {
    const preferred = this.isArabic() ? ar : en;
    return String(preferred || (this.isArabic() ? en : ar) || fallback || '').trim();
  }

  private createInvoiceItemGroup(): UntypedFormGroup {
    return this.formBuilder.group({
      carId: [null, Validators.required],
      colorId: [null],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      lineDiscount: [0, [Validators.min(0)]],
      lineNotes: ['', [Validators.maxLength(300)]]
    });
  }

  private ensureCarResources(carId: number, onLoaded?: () => void): void {
    if (!this.colorsByCarId.has(carId)) {
      this.carCarColorService.getByCarId(carId).pipe(first()).subscribe({
        next: (colors) => {
          this.colorsByCarId.set(carId, (colors || []).filter(item => item.isAvailable !== false));
          onLoaded?.();
        },
        error: (error) => this.showError(error)
      });
    } else {
      onLoaded?.();
    }

    if (!this.imageByCarId.has(carId)) {
      this.carService.getCarImages(carId).pipe(first()).subscribe({
        next: (images) => {
          const primaryImage = (images || []).find(item => item.isPrimary) || (images || [])[0];
          this.imageByCarId.set(carId, primaryImage?.imageUrl || null);
        },
        error: () => {
          this.imageByCarId.set(carId, null);
        }
      });
    }
  }

  private applyCustomerUserFilters(): void {
    const term = this.customerUserSearchTerm.trim().toLowerCase();
    if (!term) {
      this.filteredCustomerUsers = [...this.customerUsers];
      return;
    }

    this.filteredCustomerUsers = this.customerUsers.filter(user => {
      const searchable = [
        user.userName,
        user.email,
        user.mobileNo,
        user.nameAr,
        user.nameEn,
        user.fullNameAr,
        user.fullNameEn
      ]
        .filter(Boolean)
        .join(' ')
        .toLowerCase();

      return searchable.includes(term);
    });
  }

  private applyDefaultBranch(): void {
    if (!this.currentUserBranchId) {
      return;
    }

    const branchControl = this.invoiceForm.get('branchId');
    if (!branchControl) {
      return;
    }

    const hasSelectedBranch = Number(branchControl.value) > 0;
    const branchExists = this.branches.some(item => item.id === this.currentUserBranchId);
    if (!hasSelectedBranch && branchExists) {
      branchControl.setValue(this.currentUserBranchId);
    }
  }

  private applySuggestedPrice(index: number): void {
    const group = this.items.at(index) as UntypedFormGroup;
    const carId = Number(group.get('carId')?.value);
    const colorId = Number(group.get('colorId')?.value);

    if (!carId || !colorId) {
      return;
    }

    const selectedColor = this.getAvailableColors(carId).find(item => item.colorId === colorId);
    const suggestedPrice = this.resolveSuggestedPrice(selectedColor);

    group.patchValue(
      {
        unitPrice: suggestedPrice
      },
      { emitEvent: false }
    );
  }

  private resolveSuggestedPrice(item?: CarCarColor): number {
    if (!item) {
      return 0;
    }

    return this.toNumber(item.pricingPerColor) ||
      this.toNumber(item.totalPrice) ||
      this.toNumber(item.pricePefore) ||
      0;
  }

  private getLineBaseAmount(control: AbstractControl): number {
    const quantity = this.toNumber(control.get('quantity')?.value, 1);
    const unitPrice = this.toNumber(control.get('unitPrice')?.value);
    return quantity * unitPrice;
  }

  private getLineDiscount(control: AbstractControl): number {
    const discount = this.toNumber(control.get('lineDiscount')?.value);
    const baseAmount = this.getLineBaseAmount(control);
    return discount > baseAmount ? baseAmount : discount;
  }

  getLineVatAmount(control: AbstractControl): number {
    const carId = Number(control.get('carId')?.value);
    const colorId = Number(control.get('colorId')?.value);
    const quantity = this.toNumber(control.get('quantity')?.value, 1);
    const baseAfterDiscount = this.getLineBaseAmount(control) - this.getLineDiscount(control);
    if (baseAfterDiscount <= 0 || !carId) {
      return 0;
    }

    const selectedColor = this.getColorPreview(carId, colorId);
    const explicitVat = this.toNumber(selectedColor?.vatAmount);
    if (explicitVat > 0) {
      return explicitVat * quantity;
    }

    const car = this.getCar(carId);
    const vatRate = this.toNumber(car?.vat);
    return vatRate > 0 ? (baseAfterDiscount * vatRate) / 100 : 0;
  }

  private resolveImageUrl(imageUrl?: string | null, fallback = this.fallbackCarImage): string {
    if (!imageUrl) {
      return fallback;
    }

    if (
      imageUrl.startsWith('http://') ||
      imageUrl.startsWith('https://') ||
      imageUrl.startsWith('data:') ||
      imageUrl.startsWith('assets/')
    ) {
      return imageUrl;
    }

    if (imageUrl.startsWith('/')) {
      return `${GlobalComponent.API_URL}${imageUrl}`;
    }

    return `${GlobalComponent.API_URL}/${imageUrl}`;
  }

  private generateInvoiceNumber(): string {
    const now = new Date();
    const datePart = `${now.getFullYear()}${this.pad(now.getMonth() + 1)}${this.pad(now.getDate())}`;
    const timePart = `${this.pad(now.getHours())}${this.pad(now.getMinutes())}`;
    return `INV-${datePart}-${timePart}`;
  }

  private getNowInputValue(): string {
    return this.toDateTimeLocalValue(new Date());
  }

  private getFutureInputValue(days: number): string {
    const value = new Date();
    value.setDate(value.getDate() + days);
    return this.toDateTimeLocalValue(value);
  }

  private toDateTimeLocalValue(date: Date): string {
    const offset = date.getTimezoneOffset();
    const normalized = new Date(date.getTime() - offset * 60000);
    return normalized.toISOString().slice(0, 16);
  }

  private pad(value: number): string {
    return value.toString().padStart(2, '0');
  }

  private isArabic(): boolean {
    return (this.translate.currentLang || document.documentElement.lang || 'en').toLowerCase().startsWith('ar');
  }

  private toNumber(value: unknown, fallback = 0): number {
    const result = Number(value);
    return Number.isFinite(result) ? result : fallback;
  }

  private normalizeString(value: unknown): string | null {
    const normalized = String(value || '').trim();
    return normalized ? normalized : null;
  }

  private normalizeUserId(value: unknown): string | null {
    const normalized = this.normalizeString(value);
    return normalized ? normalized : null;
  }

  private openInvoicePrintTab(invoiceId: number, autoPrint = false): void {
    if (!invoiceId) {
      return;
    }

    const tree = this.router.createUrlTree(['/admin/invoices/print-report', invoiceId], {
      queryParams: autoPrint ? { autoPrint: 1 } : undefined
    });
    const url = this.router.serializeUrl(tree);
    window.open(url, '_blank');
  }

  private showError(error: unknown): void {
    this.toastService.show(getErrorMessage(error), {
      classname: 'bg-danger text-white',
      nativeToast: true
    });
  }
}
