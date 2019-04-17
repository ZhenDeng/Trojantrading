import { ShoppingItem } from './shoppingItem';
export interface Product {
    id: number
    name: string
    originalPrice: number
    agentPrice: number
    resellerPrice: number
    category: string
    button: string
    quantity:number
    status: string
    shoppingItem: ShoppingItem
}