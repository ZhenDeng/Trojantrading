import { User } from './user';
import { ShoppingItem } from './shoppingItem';
export interface ShoppingCart {
    Id: number
    totalItems: number
    totalPrice: number
    originalPrice: number
    userId: number
    user: User
    shoppingItems: ShoppingItem[]
}