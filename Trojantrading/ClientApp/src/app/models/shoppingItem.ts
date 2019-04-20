import { Product } from "./Product";

export interface ShoppingItem {
    amount: number
    product: Product
    subTotal: number
}