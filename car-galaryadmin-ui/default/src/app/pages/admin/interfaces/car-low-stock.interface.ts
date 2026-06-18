export interface CarLowStockAlert {
  carId: number;
  carNameAr?: string | null;
  carNameEn?: string | null;
  colorId?: number | null;
  colorNameAr?: string | null;
  colorNameEn?: string | null;
  remainingStockQuantity: number;
  thresholdQuantity: number;
  invoiceId: number;
  invoiceNumber: string;
}
