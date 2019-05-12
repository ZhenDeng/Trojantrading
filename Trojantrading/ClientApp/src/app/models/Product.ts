import { ShoppingItem } from './shoppingItem';

export interface Product {
    id: number
    name: string
    originalPrice: number
    agentPrice: number
    wholesalerPrice: number
    prepaymentDiscount: number
    category: string
    button: string
    quantity: number
    status: string
    shoppingItem: ShoppingItem[]
}

export interface Category{
    type: string
    category: string
}