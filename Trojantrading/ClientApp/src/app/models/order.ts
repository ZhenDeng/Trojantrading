import { User } from './user';
import { ShoppingItem } from './shoppingItem';

export interface Order {
    id: number
    createdDate: Date
    totalItems: number
    totalPrice: number
    balance: number
    orderStatus: string
    clientMessage: string
    adminMessage: string
    user: User
    shoppingItems: ShoppingItem[]  
}