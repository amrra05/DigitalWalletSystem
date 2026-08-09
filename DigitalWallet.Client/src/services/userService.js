import apiClient from "../api/apiClient";


export async function getCurrentUser()
{

    const response =
        await apiClient.get(
            "/auth/me"
        );


    return response.data;

}