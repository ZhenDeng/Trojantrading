import { ShoppingItem } from './shoppingItem';
import { ShoppingCart } from "./shoppingCart";

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
    billingStreetNumber: string
    billingAddressLine: string
    billingSuburb: string
    billingState: string
    billingPostCode: string
    shippingCustomerName: string
    shippingStreetNumber: string
    shippingAddressLine: string
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
    shoppingItems: ShoppingItem[]
}