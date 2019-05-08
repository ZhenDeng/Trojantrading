import { ShoppingCart } from './shoppingCart';
import { User } from './user';

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
    userId: number
    shoppingCartId: number
    shoppingCart: ShoppingCart  
    customer: string
    invoiceNo: string
}

export interface Status{
    type: string
}