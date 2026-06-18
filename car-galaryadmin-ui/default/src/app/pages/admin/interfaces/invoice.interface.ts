export interface InvoiceDetail {
  id: number;
  carId: number;
  carNameAr?: string | null;
  carNameEn?: string | null;
  modelNameAr?: string | null;
  modelNameEn?: string | null;
  brandNameAr?: string | null;
  brandNameEn?: string | null;
  plateNumberAr?: string | null;
  plateNumberEn?: string | null;
  year: number;
  mileage: number;
  primaryImageUrl?: string | null;
  colorId?: number | null;
  colorNameAr?: string | null;
  colorNameEn?: string | null;
  colorCode?: string | null;
  quantity: number;
  unitPrice: number;
  discountAmount: number;
  vatAmount: number;
  totalAmount: number;
  notes?: string | null;
}

export interface Invoice {
  id: number;
  userId?: string | null;
  userFullNameAr?: string | null;
  userFullNameEn?: string | null;
  userPhoneNumber?: string | null;
  userEmail?: string | null;
  branchId: number;
  branchNameAr?: string | null;
  branchNameEn?: string | null;
  paymentMethod: number;
  paymentMethodNameAr?: string | null;
  paymentMethodNameEn?: string | null;
  invoiceNumber: string;
  issueDate: string;
  dueDate: string;
  customerName: string;
  customerPhone: string;
  customerEmail?: string | null;
  customerAddress?: string | null;
  notes?: string | null;
  subtotal: number;
  vatTotal: number;
  shippingFee: number;
  extraDiscount: number;
  grandTotal: number;
  zatcaQrCode?: string | null;
  createdBy?: string | null;
  updatedBy?: string | null;
  createdAt: string;
  updatedAt?: string | null;
  isAvailable: boolean;
  details: InvoiceDetail[];
}

export interface CreateInvoiceDetail {
  carId: number;
  colorId?: number | null;
  quantity: number;
  unitPrice: number;
  discountAmount: number;
  vatAmount: number;
  notes?: string | null;
}

export interface CreateInvoice {
  userId?: string | null;
  branchId: number;
  paymentMethod: number;
  invoiceNumber: string;
  issueDate: string;
  dueDate: string;
  customerName: string;
  customerPhone: string;
  customerEmail?: string | null;
  customerAddress?: string | null;
  notes?: string | null;
  shippingFee: number;
  extraDiscount: number;
  details: CreateInvoiceDetail[];
}

export interface UpdateInvoiceDetail extends CreateInvoiceDetail {}

export interface UpdateInvoice extends CreateInvoice {
  isAvailable?: boolean;
  details: UpdateInvoiceDetail[];
}
