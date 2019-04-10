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
    shippingAddress: UserAddress
    billingAddress: UserAddress
    // shoppingCart: ShoppingCart
    // orders: Order[]
    // UserRoles: UserRole[]
    // ShoppingItems: ShoppingItem[]
}