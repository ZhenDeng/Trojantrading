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
    shoppingCart: ShoppingCart
    shoppingCartId: number
    invoiceNo: string
}

export interface Status{
    orderStatus: string
}