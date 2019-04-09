import { ShoppingCart } from './shoppingCart';
import { Product } from "./Product";
import { Order } from "./order";

export interface ShoppingItem {
    Id: number
    amount: number
    product: Product
    order: Order
    orderId: number
    shoppingCart: ShoppingCart
    shoppingCartId: number
}