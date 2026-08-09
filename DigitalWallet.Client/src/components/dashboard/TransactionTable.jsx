import "./DashboardComponents.css";


function TransactionTable({ transactions })
{

    const recentTransactions =
        transactions.slice(0, 5);



    return (

        <div className="transactions-card">


            <h2>
                Recent Transactions
            </h2>



            <table>


                <thead>

                    <tr>

                        <th>Date</th>

                        <th>Type</th>

                        <th>Amount</th>

                        <th>Status</th>

                    </tr>

                </thead>



                <tbody>


                {

                    recentTransactions.length === 0

                    ?

                    <tr>

                        <td colSpan="4">
                            No transactions found
                        </td>

                    </tr>


                    :


                    recentTransactions.map(transaction => (


                        <tr key={transaction.id}>


                            <td>

                                {
                                    new Date(
                                        transaction.createdAt
                                    ).toLocaleDateString()
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


                    ))

                }


                </tbody>


            </table>


        </div>

    );

}


export default TransactionTable;