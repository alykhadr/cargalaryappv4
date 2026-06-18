import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { first } from 'rxjs/operators';
import { GlobalComponent } from 'src/app/global-component';
import { PaginationService } from 'src/app/core/services/pagination.service';
import { ToastService } from '../../icons/toast-service';
import { Invoice } from '../interfaces/invoice.interface';
import { InvoiceService } from '../services/invoice.service';
import { getErrorMessage } from '../shared/error-message.util';

@Component({
  selector: 'app-invoice-list-page',
  templateUrl: './invoice-list-page.component.html',
  styleUrl: './invoice-list-page.component.scss',
  standalone: false
})
export class InvoiceListPageComponent implements OnInit {
  breadCrumbItems!: Array<{}>;
  invoices: Invoice[] = [];
  filteredInvoices: Invoice[] = [];
  pagedInvoices: Invoice[] = [];
  selectedInvoice?: Invoice;
  searchTerm = '';
  isLoading = false;
  isDetailsModalOpen = false;

  constructor(
    public service: PaginationService,
    private invoiceService: InvoiceService,
    private router: Router,
    private toastService: ToastService,
    private translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: this.translate.instant('MENUITEMS.REQUEST.TEXT') },
      { label: this.translate.instant('MENUITEMS.REQUEST.LIST.INVOICES'), active: true }
    ];

    this.service.pageSize = 10;
    this.loadInvoices();
  }

  get totalAmount(): number {
    return this.filteredInvoices.reduce((sum, item) => sum + (item.grandTotal || 0), 0);
  }

  get totalCarsCount(): number {
    return this.filteredInvoices.reduce((sum, item) => sum + (item.details?.length || 0), 0);
  }

  loadInvoices(): void {
    this.isLoading = true;
    this.invoiceService.getAll().pipe(first()).subscribe({
      next: (invoices) => {
        this.invoices = invoices || [];
        this.applyFilters(true);
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.showError(error);
      }
    });
  }

  onSearch(): void {
    this.applyFilters(true);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.applyFilters(true);
  }

  onPageChange(page: number): void {
    this.service.page = page;
    this.pagedInvoices = this.service.changePage(this.filteredInvoices);
  }

  openDetails(invoice: Invoice): void {
    this.selectedInvoice = invoice;
    this.isDetailsModalOpen = true;
  }

  closeDetails(): void {
    this.isDetailsModalOpen = false;
    this.selectedInvoice = undefined;
  }

  printInvoice(invoiceId: number, autoPrint = false): void {
    if (!invoiceId) {
      return;
    }

    const tree = this.router.createUrlTree(['/admin/invoices/print-report', invoiceId], {
      queryParams: autoPrint ? { autoPrint: 1 } : undefined
    });
    const url = this.router.serializeUrl(tree);
    window.open(url, '_blank');
  }

  getLocalizedValue(ar?: string | null, en?: string | null, fallback = ''): string {
    const isArabic = (this.translate.currentLang || document.documentElement.lang || 'en').toLowerCase().startsWith('ar');
    const preferred = isArabic ? ar : en;
    return String(preferred || (isArabic ? en : ar) || fallback || '').trim();
  }

  getBranchDisplayName(invoice: Invoice): string {
    return this.getLocalizedValue(invoice.branchNameAr, invoice.branchNameEn, this.translate.instant('COMMON.NOT_AVAILABLE'));
  }

  getInvoiceUserDisplayName(invoice: Invoice): string {
    return this.getLocalizedValue(
      invoice.userFullNameAr,
      invoice.userFullNameEn,
      this.translate.instant('COMMON.NOT_AVAILABLE')
    );
  }

  getPaymentMethodDisplayName(invoice: Invoice): string {
    return this.getLocalizedValue(invoice.paymentMethodNameAr, invoice.paymentMethodNameEn, this.translate.instant('COMMON.NOT_AVAILABLE'));
  }

  getInvoiceCarsSummary(invoice: Invoice): string {
    const names = (invoice.details || [])
      .slice(0, 2)
      .map(item => this.getLocalizedValue(item.carNameAr, item.carNameEn, `#${item.carId}`))
      .filter(Boolean);

    if (invoice.details.length <= 2) {
      return names.join(' , ');
    }

    return `${names.join(' , ')} +${invoice.details.length - 2}`;
  }

  getInvoicePrimaryImage(invoice: Invoice): string {
    const imageUrl = invoice.details?.find(item => !!item.primaryImageUrl)?.primaryImageUrl;
    return this.resolveImageUrl(imageUrl, 'assets/images/car-placeholder.png');
  }

  getDetailImageUrl(imageUrl?: string | null): string {
    return this.resolveImageUrl(imageUrl, 'assets/images/car-placeholder.png');
  }

  private applyFilters(resetPage = false): void {
    const term = this.searchTerm.trim().toLowerCase();
    let data = [...this.invoices];

    if (term) {
      data = data.filter(invoice => {
        const invoiceCars = (invoice.details || [])
          .map(item => `${item.carNameAr || ''} ${item.carNameEn || ''} ${item.brandNameAr || ''} ${item.brandNameEn || ''}`)
          .join(' ')
          .toLowerCase();

        return (
          (invoice.invoiceNumber || '').toLowerCase().includes(term) ||
          (invoice.customerName || '').toLowerCase().includes(term) ||
          (invoice.customerPhone || '').toLowerCase().includes(term) ||
          (invoice.customerEmail || '').toLowerCase().includes(term) ||
          this.getInvoiceUserDisplayName(invoice).toLowerCase().includes(term) ||
          (invoice.createdBy || '').toLowerCase().includes(term) ||
          this.getBranchDisplayName(invoice).toLowerCase().includes(term) ||
          invoiceCars.includes(term)
        );
      });
    }

    this.filteredInvoices = data;
    if (resetPage) {
      this.service.page = 1;
    }
    this.pagedInvoices = this.service.changePage(this.filteredInvoices);
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

  private showError(error: unknown): void {
    this.toastService.show(getErrorMessage(error), {
      classname: 'bg-danger text-white',
      nativeToast: true
    });
  }
}
