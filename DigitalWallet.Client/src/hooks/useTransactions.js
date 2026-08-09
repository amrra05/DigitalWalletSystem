import { useEffect, useState } from "react";
import { getTransactions } from "../services/transactionService";


function useTransactions()
{

    const [transactions,setTransactions] = useState([]);

    const [loading,setLoading] = useState(true);



    useEffect(() =>
    {

        async function loadTransactions()
        {

            try
            {

                const data =
                    await getTransactions();


                setTransactions(data);

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


        loadTransactions();


    },[]);



    return {

        transactions,

        loading

    };

}


export default useTransactions;