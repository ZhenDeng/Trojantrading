import { User } from './user';
import { ShoppingItem } from './shoppingItem';
import { Order } from './order';

export interface ShoppingCart {
    id: number
    totalItems: number
    totalPrice: number
    originalPrice: number
    userId: number
    user: User
    order: Order
    status: string
    shoppingItems: ShoppingItem[]
    note: string
}