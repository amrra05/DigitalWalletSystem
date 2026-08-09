import { useEffect, useState } from "react";
import { getAdminDashboard } from "../services/adminService";
import { Link } from "react-router-dom";
import Skeleton from "../components/Skeleton/Skeleton";
import LogoutButton from "../components/LogoutButton";
import "./styles/AdminDashboard.css";


function AdminDashboard()
{

    const [dashboard,setDashboard] = useState(null);

    const [loading,setLoading] = useState(true);



    useEffect(() =>
    {

        async function loadDashboard()
        {

            try
            {

                const data =
                    await getAdminDashboard();


                setDashboard(data);

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


        loadDashboard();


    },[]);





    return (

        <div className="admin-page">


            <h1>
                Admin Dashboard
            </h1>

<LogoutButton />

            <div className="stats-container">


                {
                loading ?

                <>
                    <Skeleton height="120px"/>
                    <Skeleton height="120px"/>
                    <Skeleton height="120px"/>
                    <Skeleton height="120px"/>
                </>

                :

                <>

                <div className="stat-card">

                    <h3>
                        Total Users
                    </h3>

                    <p>
                        {dashboard.totalUsers}
                    </p>

                </div>



                <div className="stat-card">

                    <h3>
                        Active Users
                    </h3>

                    <p>
                        {dashboard.activeUsers}
                    </p>

                </div>




                <div className="stat-card">

                    <h3>
                        Transactions
                    </h3>

                    <p>
                        {dashboard.totalTransactions}
                    </p>

                </div>




                <div className="stat-card">

                    <h3>
                        Total Balance
                    </h3>

                    <p>
                        {dashboard.totalWalletBalance}
                        {" "}
                        JOD
                    </p>

                </div>


                </>

                }


            </div>


            <div className="quick-actions">

                <h2>Quick Actions</h2>

                <div className="actions-grid">

                    <Link
                        to="/admin/users"
                        className="action-card"
                    >
                        👥
                        <span>Manage Users</span>
                    </Link>

                    <Link
                        to="/admin/transactions"
                        className="action-card"
                    >
                        💸
                        <span>Transactions</span>
                    </Link>

                    <Link
                        to="/admin/auditlogs"
                        className="action-card"
                    >
                        📋
                        <span>Audit Logs</span>
                    </Link>

                </div>

            </div>



            <div className="recent-section">


                <h2>
                    Recent Transactions
                </h2>



                {
                loading ?

                <>
                    <Skeleton height="60px"/>
                    <Skeleton height="60px"/>
                    <Skeleton height="60px"/>
                </>


                :

                dashboard.recentTransactions.map(
                    transaction =>

                    <div
                    className="transaction-card"
                    key={transaction.id}
                    >

                        <span>
                            {transaction.senderEmail}
                        </span>


                        <strong>
                            →
                        </strong>


                        <span>
                            {transaction.receiverEmail}
                        </span>


                        <b>
                            {transaction.amount}
                            {" "}
                            JOD
                        </b>


                    </div>

                )

                }



            </div>



        </div>

    );

}


export default AdminDashboard;