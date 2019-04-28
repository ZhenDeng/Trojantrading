import { Product } from "./Product";

export interface ShoppingItem {
    id: number
    amount: number
    product: Product
    subTotal: number
}