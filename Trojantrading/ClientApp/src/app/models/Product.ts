import { ShoppingItem } from './shoppingItem';
export interface Product {
    Id: number,
    createdDate: Date,
    name: string,
    originalPrice: number,
    vipOnePrice: number,
    vipTwoPrice: number,
    category: string,
    shoppingItem: ShoppingItem
    shoppingItemId: number
}