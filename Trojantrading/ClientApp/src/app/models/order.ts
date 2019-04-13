import { User } from './user';
import { ShoppingItem } from './shoppingItem';

export interface Order {
    Id: number
    createdDate: Date
    totalItems: number
    totalPrice: number
    balance: number
    orderStatus: string
    clientMessage: string
    adminMessage: string
    user: User
    userId: number
    shoppingItems: ShoppingItem[]  
}