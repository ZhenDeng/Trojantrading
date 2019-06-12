import { Product } from "./Product";

export interface ShoppingItem {
    id: number
    amount: number
    packaging: string
    product: Product
    subTotal: number
}