import { ShoppingItem } from './shoppingItem';
import { ShoppingCart } from "./shoppingCart";
import { Order } from './order';

export interface User {
    id: number
    createdDate: Date
    account: string
    passswordHash: string
    password: string
    bussinessName: string
    trn: string
    email: string
    mobile: string
    phone: string
    status: string
    sendEmail: boolean
    billingCustomerName: string
    billingAddressLine1: string
    billingAddressLine2: string
    billingAddressLine3: string
    billingSuburb: string
    billingState: string
    billingPostCode: string
    shippingCustomerName: string
    shippingAddressLine1: string
    shippingAddressLine2: string
    shippingAddressLine3: string
    shippingSuburb: string
    shippingState: string
    shippingPostCode: string
    companyAddress: string
    companyEmail: string
    companyPhone: string
    fax: string
    abn: string
    acn: string
    role: string
    shoppingCarts: ShoppingCart[]
    orders: Order[]
    shoppingItems: ShoppingItem[]
}