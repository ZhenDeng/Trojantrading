import { ShoppingItem } from './shoppingItem';
import { ShoppingCart } from "./shoppingCart";
import { Order } from './order';

export interface User {
    Id: number
    createdDate: Date
    account: string
    password: string
    level: string
    bussinessName: string
    address: string
    postCode: string
    abn: string
    trn: string
    email: string
    phone: string
    status: string
    sendEmail: boolean
    shoppingCart: ShoppingCart
    orders: Order[]
    // UserRoles: UserRole[]
    shoppingItems: ShoppingItem[]
}