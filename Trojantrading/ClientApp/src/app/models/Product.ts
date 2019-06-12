export interface Product {
    id: number
    itemCode: string
    name: string
    packaging: string
    originalPrice: number
    agentPrice: number
    wholesalerPrice: number
    prepaymentDiscount: number
    category: string
    button: string
    quantity: number
    status: string
    packagingLists: PackagingList[]
}

export interface Category{
    type: string
    category: string
}

export interface PackagingList{
    packageName: string
    productId: number
}