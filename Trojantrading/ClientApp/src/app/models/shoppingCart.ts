import { User } from './user';
import { ShoppingItem } from './shoppingItem';

export interface ShoppingCart {
    id: number
    totalItems: number
    totalPrice: number
    originalPrice: number
    shoppingItems: ShoppingItem[]
}