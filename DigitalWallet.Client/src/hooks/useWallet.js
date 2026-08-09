import { useEffect, useState } from "react";
import { getWallet } from "../services/walletService";


function useWallet()
{

    const [wallet,setWallet] = useState(null);

    const [loading,setLoading] = useState(true);



    useEffect(() =>
    {

        async function loadWallet()
        {

            try
            {

                const data =
                    await getWallet();


                setWallet(data);

            }
            catch(error)
            {

                console.log(error);

            }
            finally
            {

                setLoading(false);

            }

        }


        loadWallet();


    },[]);



    return {
        wallet,
        loading
    };

}


export default useWallet;