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
    shippingAddressId: number
    billingAddress: UserAddress
    billingAddressId: number
    shoppingItems: ShoppingItem[]
    role: Role
    roleId: number
}