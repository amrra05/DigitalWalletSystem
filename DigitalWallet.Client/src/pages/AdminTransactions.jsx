import { useEffect, useState } from "react";

import { getAdminTransactions } from "../services/adminService";

import Skeleton from "../components/Skeleton/Skeleton";

import BackButton from "../components/BackButton";

import "./styles/AdminTransactions.css";


function AdminTransactions()
{

    const [transactions, setTransactions] =
        useState([]);


    const [loading, setLoading] =
        useState(true);


    const [page, setPage] =
        useState(1);


    const [totalPages, setTotalPages] =
        useState(1);


    const [search, setSearch] =
        useState("");


    const [type, setType] =
        useState("");


    const [status, setStatus] =
        useState("");


    const [selectedTransaction, setSelectedTransaction] =
        useState(null);



    async function loadTransactions()
    {

        try
        {

            setLoading(true);


            const data =
                await getAdminTransactions({

                    page,

                    pageSize: 10,

                    search,

                    type,

                    status

                });


            setTransactions(
                data.items
            );


            setTotalPages(
                data.totalPages
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

    }, [page, search, type, status]);



    return (

        <div className="admin-transactions-page">


            <h1>
                Transactions
            </h1>


            <BackButton />



            <div className="transactions-toolbar">


                <input

                    className="transactions-search"

                    placeholder="Search by sender or receiver..."

                    value={search}

                    onChange={(e) =>
                    {

                        setPage(1);

                        setSearch(
                            e.target.value
                        );

                    }}

                />



                <select

                    value={type}

                    onChange={(e) =>
                    {

                        setPage(1);

                        setType(
                            e.target.value
                        );

                    }}

                >

                    <option value="">
                        All Types
                    </option>

                    <option value="Transfer">
                        Transfer
                    </option>

                    <option value="Deposit">
                        Deposit
                    </option>

                    <option value="Withdrawal">
                        Withdrawal
                    </option>

                </select>



                <select

                    value={status}

                    onChange={(e) =>
                    {

                        setPage(1);

                        setStatus(
                            e.target.value
                        );

                    }}

                >

                    <option value="">
                        All Statuses
                    </option>

                    <option value="Completed">
                        Completed
                    </option>

                    <option value="Pending">
                        Pending
                    </option>

                    <option value="Failed">
                        Failed
                    </option>

                </select>


            </div>



            {
                loading ?

                <>

                    <Skeleton height="60px" />

                    <Skeleton height="60px" />

                    <Skeleton height="60px" />

                    <Skeleton height="60px" />

                    <Skeleton height="60px" />

                </>

                :


                <table className="transactions-table">


                    <thead>

                        <tr>

                            <th>
                                ID
                            </th>

                            <th>
                                Sender
                            </th>

                            <th>
                                Receiver
                            </th>

                            <th>
                                Amount
                            </th>

                            <th>
                                Type
                            </th>

                            <th>
                                Status
                            </th>

                            <th>
                                Date
                            </th>

                            <th>
                                Action
                            </th>

                        </tr>

                    </thead>



                    <tbody>


                    {
                        transactions.length === 0 ?

                        <tr>

                            <td
                                colSpan="8"
                                className="no-transactions"
                            >

                                No transactions found.

                            </td>

                        </tr>

                        :


                        transactions.map(
                            transaction =>

                            <tr
                                key={transaction.id}
                            >

                                <td>
                                    {transaction.id}
                                </td>


                                <td>
                                    {transaction.senderEmail}
                                </td>


                                <td>
                                    {transaction.receiverEmail}
                                </td>


                                <td className="amount">

                                    {transaction.amount}
                                    {" "}
                                    JOD

                                </td>


                                <td>
                                    {transaction.type}
                                </td>


                                <td>

                                    <span
                                        className={
                                            `transaction-status ${
                                                transaction.status
                                                    ?.toLowerCase()
                                            }`
                                        }
                                    >

                                        {
                                            transaction.status
                                        }

                                    </span>

                                </td>


                                <td>

                                    {
                                        new Date(
                                            transaction.createdAt
                                        )
                                        .toLocaleDateString()
                                    }

                                </td>


                                <td>

                                    <button

                                        className="view-btn"

                                        onClick={() =>
                                            setSelectedTransaction(
                                                transaction
                                            )
                                        }

                                    >

                                        View

                                    </button>

                                </td>

                            </tr>

                        )
                    }


                    </tbody>


                </table>

            }



            <div className="pagination">


                <button

                    disabled={
                        page === 1
                    }

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

                    disabled={
                        page === totalPages
                    }

                    onClick={() =>
                        setPage(page + 1)
                    }

                >

                    Next

                </button>


            </div>



            {
                selectedTransaction &&

                <div className="modal-overlay">

                    <div className="transaction-modal">


                        <h2>
                            Transaction Details
                        </h2>


                        <p>
                            <strong>ID:</strong>{" "}
                            {selectedTransaction.id}
                        </p>


                        <p>
                            <strong>Sender:</strong>{" "}
                            {selectedTransaction.senderEmail}
                        </p>


                        <p>
                            <strong>Receiver:</strong>{" "}
                            {selectedTransaction.receiverEmail}
                        </p>


                        <p>
                            <strong>Amount:</strong>{" "}
                            {selectedTransaction.amount} JOD
                        </p>


                        <p>
                            <strong>Type:</strong>{" "}
                            {selectedTransaction.type}
                        </p>


                        <p>
                            <strong>Status:</strong>{" "}
                            {selectedTransaction.status}
                        </p>


                        <p>
                            <strong>Date:</strong>{" "}
                            {
                                new Date(
                                    selectedTransaction.createdAt
                                )
                                .toLocaleString()
                            }
                        </p>



                        <button

                            className="close-btn"

                            onClick={() =>
                                setSelectedTransaction(null)
                            }

                        >

                            Close

                        </button>


                    </div>

                </div>

            }


        </div>

    );

}


export default AdminTransactions;