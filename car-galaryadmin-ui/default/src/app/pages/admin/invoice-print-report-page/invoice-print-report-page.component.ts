import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { of } from 'rxjs';
import { catchError, first } from 'rxjs/operators';
import { GlobalComponent } from 'src/app/global-component';
import { CompanyInfo } from '../interfaces/company-info.interface';
import { Invoice } from '../interfaces/invoice.interface';
import { CompanyInfoService } from '../services/company-info.service';
import { InvoiceService } from '../services/invoice.service';
import { getErrorMessage } from '../shared/error-message.util';

@Component({
  selector: 'app-invoice-print-report-page',
  templateUrl: './invoice-print-report-page.component.html',
  styleUrl: './invoice-print-report-page.component.scss',
  standalone: false
})
export class InvoicePrintReportPageComponent implements OnInit {
  breadCrumbItems!: Array<{}>;

  isLoading = true;
  errorMessage = '';
  invoice: Invoice | null = null;
  companyInfo: CompanyInfo | null = null;
  generatedAt = new Date();

  private autoPrintHandled = false;
  private previousDocumentTitle = '';
  private hasTemporaryPrintTitle = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private translate: TranslateService,
    private invoiceService: InvoiceService,
    private companyInfoService: CompanyInfoService
  ) {}

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: this.translate.instant('MENUITEMS.REQUEST.TEXT') },
      { label: this.translate.instant('INVOICE_PAGE.PRINT_REPORT'), active: true }
    ];

    const invoiceId = Number(this.route.snapshot.paramMap.get('id'));
    if (!Number.isFinite(invoiceId) || invoiceId <= 0) {
      this.isLoading = false;
      this.errorMessage = this.translate.instant('INVOICE_PAGE.NO_PRINT_DATA');
      return;
    }

    this.loadReportData(invoiceId);
  }

  loadReportData(invoiceId: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.invoiceService.getById(invoiceId).pipe(first()).subscribe({
      next: (invoice) => {
        this.invoice = invoice;
        this.loadCompanyInfo();
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = getErrorMessage(error, this.translate.instant('INVOICE_PAGE.NO_PRINT_DATA'));
      }
    });
  }

  private loadCompanyInfo(): void {
    this.companyInfoService.getCompanyInfos().pipe(
      catchError(() => of([] as CompanyInfo[])),
      first()
    ).subscribe({
      next: (companyInfos) => {
        const companyList = companyInfos || [];
        this.companyInfo = companyList.find(x => x.isAvailable) || companyList[0] || null;
        this.generatedAt = new Date();
        this.isLoading = false;

        if (this.shouldAutoPrint()) {
          this.removeAutoPrintQueryParam();
          this.autoPrintHandled = true;
          setTimeout(() => this.printReport(), 250);
        }
      },
      error: () => {
        this.generatedAt = new Date();
        this.isLoading = false;
      }
    });
  }

  backToList(): void {
    this.router.navigate(['/admin/invoices/list']);
  }

  printReport(): void {
    this.applyPrintDocumentTitle();
    const onAfterPrint = () => this.restoreDocumentTitle();
    window.addEventListener('afterprint', onAfterPrint, { once: true });
    window.print();
    setTimeout(() => this.restoreDocumentTitle(), 2000);
  }

  get totalQuantity(): number {
    return (this.invoice?.details || []).reduce((sum, detail) => sum + (detail.quantity || 0), 0);
  }

  get lineDiscountTotal(): number {
    return (this.invoice?.details || []).reduce((sum, detail) => sum + (detail.discountAmount || 0), 0);
  }

  get linkedUserDisplayName(): string {
    if (!this.invoice) {
      return '-';
    }

    return this.getLocalizedValue(this.invoice.userFullNameAr, this.invoice.userFullNameEn, '-');
  }

  get hasLinkedUser(): boolean {
    return !!this.invoice?.userId;
  }

  getLocalizedValue(ar?: string | null, en?: string | null, fallback = '-'): string {
    const isArabic = (this.translate.currentLang || document.documentElement.lang || 'en').toLowerCase().startsWith('ar');
    const preferred = isArabic ? ar : en;
    const alternate = isArabic ? en : ar;
    return preferred || alternate || fallback;
  }

  getLineItemName(carNameAr?: string | null, carNameEn?: string | null, carId?: number | null): string {
    return this.getLocalizedValue(carNameAr, carNameEn, carId ? `#${carId}` : '-');
  }

  getLineItemBrandModel(
    brandNameAr?: string | null,
    brandNameEn?: string | null,
    modelNameAr?: string | null,
    modelNameEn?: string | null
  ): string {
    const brand = this.getLocalizedValue(brandNameAr, brandNameEn, '');
    const model = this.getLocalizedValue(modelNameAr, modelNameEn, '');
    const parts = [brand, model].filter(Boolean);
    return parts.length ? parts.join(' / ') : '-';
  }

  getDetailImageUrl(imageUrl?: string | null): string {
    return this.resolveImageUrl(imageUrl, 'assets/images/car-placeholder.png');
  }

  getCompanyLogoUrl(): string {
    return this.resolveImageUrl(this.companyInfo?.logoUrl, 'assets/images/logo-sm.png');
  }

  getZatcaQrImageUrl(): string | null {
    if (!this.invoice?.id || !this.invoice?.zatcaQrCode) {
      return null;
    }

    return `${GlobalComponent.API_URL}/api/Invoices/${this.invoice.id}/zatca-qr.png`;
  }

  get hasZatcaQrCode(): boolean {
    return !!this.invoice?.zatcaQrCode;
  }

  get sellerDisplayName(): string {
    return this.getLocalizedValue(this.companyInfo?.companyNameAr, this.companyInfo?.companyNameEn);
  }

  get zatcaQrStatusMessage(): string {
    if (!this.companyInfo?.vatRegistrationNumber) {
      return this.translate.instant('INVOICE_PAGE.ZATCA_QR_MISSING_VAT');
    }

    return this.translate.instant('INVOICE_PAGE.ZATCA_QR_UNAVAILABLE');
  }

  private resolveImageUrl(imageUrl?: string | null, fallback = 'assets/images/car-placeholder.png'): string {
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

  private shouldAutoPrint(): boolean {
    if (this.autoPrintHandled) {
      return false;
    }

    const raw = (this.route.snapshot.queryParamMap.get('autoPrint') || '').toLowerCase();
    return raw === '1' || raw === 'true' || raw === 'yes';
  }

  private removeAutoPrintQueryParam(): void {
    const currentUrl = new URL(window.location.href);
    if (!currentUrl.searchParams.has('autoPrint')) {
      return;
    }

    currentUrl.searchParams.delete('autoPrint');
    const query = currentUrl.searchParams.toString();
    const nextUrl = `${currentUrl.pathname}${query ? `?${query}` : ''}${currentUrl.hash}`;
    window.history.replaceState(window.history.state, '', nextUrl);
  }

  private applyPrintDocumentTitle(): void {
    if (!this.invoice) {
      return;
    }

    if (!this.hasTemporaryPrintTitle) {
      this.previousDocumentTitle = document.title;
      this.hasTemporaryPrintTitle = true;
    }

    const reportTitle = this.translate.instant('INVOICE_PAGE.PRINT_REPORT_TITLE');
    const invoiceNumber = (this.invoice.invoiceNumber || '').trim();
    document.title = invoiceNumber ? `${reportTitle} - ${invoiceNumber}` : reportTitle;
  }

  private restoreDocumentTitle(): void {
    if (!this.hasTemporaryPrintTitle) {
      return;
    }

    document.title = this.previousDocumentTitle;
    this.hasTemporaryPrintTitle = false;
  }
}
