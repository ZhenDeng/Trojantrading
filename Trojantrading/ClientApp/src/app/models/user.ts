import { ShoppingItem } from './shoppingItem';
import { ShoppingCart } from "./shoppingCart";
import { Order } from './order';
import { UserAddress } from "./UserAddress";
import { Role } from './Role';

export interface User {
    id: number
    account: string
    password: string
    bussinessName: string
    postCode: string
    trn: string
    email: string
    phone: string
    mobile: string
    status: string
    sendEmail: boolean
    shoppingCart: ShoppingCart
    orders: Order[]
    shippingAddress: UserAddress
    billingAddress: UserAddress
    shoppingItems: ShoppingItem[]
    role: Role
}