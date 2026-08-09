import apiClient from "../api/apiClient";


export async function getTransactions()
{

    const response =
        await apiClient.get("Wallet/transactions");


    return response.data.items;

}