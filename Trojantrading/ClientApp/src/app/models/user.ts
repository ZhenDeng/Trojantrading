import { ShoppingItem } from './shoppingItem';
import { ShoppingCart } from "./shoppingCart";
import { Order } from './order';

import { UserAddress } from "./UserAddress";

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
    // orders: Order[]
    // UserRoles: UserRole[]
    shoppingItems: ShoppingItem[]
}