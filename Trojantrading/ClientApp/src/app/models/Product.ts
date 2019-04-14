import { ShoppingItem } from './shoppingItem';

export interface Product {
    id: number
    // createdDate: Date
    name: string
    originalPrice: number
    vipOnePrice: number
    vipTwoPrice: number
    category: string
    quantity: number
    // shoppingItem: ShoppingItem
    // shoppingItemId: number
    button: string
}