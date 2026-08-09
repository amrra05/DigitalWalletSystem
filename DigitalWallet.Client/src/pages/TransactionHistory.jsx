import { useEffect, useState } from "react";
import { getTransactions } from "../services/walletService";
import { useNavigate } from "react-router-dom";
import Skeleton from "../components/skeleton/Skeleton";
import "./styles/TransactionHistory.css";


function TransactionHistory()
{

    const navigate = useNavigate();


    const [transactions,setTransactions] = useState([]);


    const [page,setPage] = useState(1);


    const [totalPages,setTotalPages] = useState(1);


    const [loading,setLoading] = useState(true);



    const [filters,setFilters] = useState({

        type:"",
        status:"",
        sortBy:"date",
        sortOrder:"desc"

    });





    async function loadTransactions()
    {

        try
        {

            setLoading(true);


            const response =
                await getTransactions({

                    page,

                    pageSize:10,

                    type:filters.type,

                    status:filters.status,

                    sortBy:filters.sortBy,

                    sortOrder:filters.sortOrder

                });



            setTransactions(
                response.items
            );


            setTotalPages(
                response.totalPages
            );


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






    useEffect(() =>
    {

        loadTransactions();


    },[
        page,
        filters
    ]);







    function updateFilter(name,value)
    {

        setPage(1);


        setFilters({

            ...filters,

            [name]:value

        });

    }







    return (

        <div className="history-page">


            <div className="history-card">



                <div className="history-header">


                    <h1>
                        Transaction History
                    </h1>



                    <button

                        onClick={() =>
                            navigate("/dashboard")
                        }

                    >
                        Back

                    </button>



                </div>





                <div className="filters">



                    <select

                    value={filters.type}

                    onChange={(e)=>
                        updateFilter(
                            "type",
                            e.target.value
                        )
                    }

                    >


                        <option value="">
                            All Types
                        </option>


                        <option value="Deposit">
                            Deposit
                        </option>


                        <option value="Withdrawal">
                            Withdrawal
                        </option>


                        <option value="Transfer">
                            Transfer
                        </option>


                    </select>







                    <select

                    value={filters.status}

                    onChange={(e)=>
                        updateFilter(
                            "status",
                            e.target.value
                        )
                    }

                    >


                        <option value="">
                            All Status
                        </option>


                        <option value="Completed">
                            Completed
                        </option>


                        <option value="Failed">
                            Failed
                        </option>


                    </select>








                    <select

                    value={filters.sortBy}

                    onChange={(e)=>
                        updateFilter(
                            "sortBy",
                            e.target.value
                        )
                    }

                    >


                        <option value="date">
                            Date
                        </option>


                        <option value="amount">
                            Amount
                        </option>


                    </select>








                    <select

                    value={filters.sortOrder}

                    onChange={(e)=>
                        updateFilter(
                            "sortOrder",
                            e.target.value
                        )
                    }

                    >


                        <option value="desc">
                            Newest
                        </option>


                        <option value="asc">
                            Oldest
                        </option>


                    </select>





                </div>








                {
                    loading

                    ?

                    <>
                    <Skeleton height={70}/>
                    <Skeleton height={70}/>
                    <Skeleton height={70}/>
                    <Skeleton height={70}/>
                    </>


                    :


                    <table>


                        <thead>


                            <tr>


                                <th>
                                    Date
                                </th>


                                <th>
                                    Type
                                </th>


                                <th>
                                    Amount
                                </th>


                                <th>
                                    Status
                                </th>


                            </tr>


                        </thead>





                        <tbody>



                        {
                            transactions.length === 0


                            ?


                            <tr>


                                <td colSpan="4">

                                    No transactions found

                                </td>


                            </tr>



                            :



                            transactions.map(transaction =>


                                <tr key={transaction.id}>


                                    <td>

                                        {
                                            new Date(
                                                transaction.createdAt
                                            )
                                            .toLocaleDateString()
                                        }


                                    </td>



                                    <td>

                                        {transaction.type}

                                    </td>




                                    <td>

                                        {transaction.amount} JOD

                                    </td>




                                    <td>

                                        {transaction.status}

                                    </td>




                                </tr>


                            )

                        }



                        </tbody>



                    </table>


                }









                <div className="pagination">



                    <button

                    disabled={page === 1}

                    onClick={() =>
                        setPage(page - 1)
                    }

                    >

                        Previous

                    </button>






                    <span>

                        Page {page} of {totalPages}

                    </span>







                    <button

                    disabled={page === totalPages}

                    onClick={() =>
                        setPage(page + 1)
                    }

                    >

                        Next

                    </button>



                </div>





            </div>


        </div>

    );

}



export default TransactionHistory;