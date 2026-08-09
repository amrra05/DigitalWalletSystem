import apiClient from "../api/apiClient";


export async function getWallet()
{

    const response =
        await apiClient.get("/wallet");


    return response.data;

}

export async function transferMoney(data)
{

    const response =
        await apiClient.post(
            "/wallet/transfer",
            data
        );


    return response.data;

}

export async function depositMoney(data)
{
    const response =
        await apiClient.post(
            "/wallet/deposit",
            data
        );

    return response.data;
}

export async function withdrawMoney(data)
{
    const response =
        await apiClient.post(
            "/wallet/withdraw",
            data
        );

    return response.data;
}

export async function getTransactions(filters)
{

    const response =
        await apiClient.get(
            "/wallet/transactions",
            {
                params: filters
            }
        );


    return response.data;

}