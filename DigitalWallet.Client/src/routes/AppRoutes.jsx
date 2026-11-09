import {
    Routes,
    Route
}
from "react-router-dom";


import Login from "../pages/Login";

import Dashboard from "../pages/Dashboard";
import ProtectedRoute from "./ProtectedRoute";
import SendMoney from "../pages/SendMoney";
import Deposit from "../pages/Deposit";
import Withdraw from "../pages/Withdraw";
import TransactionHistory from "../pages/TransactionHistory";
import Settings from "../pages/Settings";
import Register from "../pages/Register";
import ForgotPassword from "../pages/ForgotPassword";
import ResetPassword from "../pages/ResetPassword";
import VerifyEmail from "../pages/VerifyEmail";
import PublicRoute from "./PublicRoute";
import AdminDashboard from "../pages/AdminDashboard";
import ManageUsers from "../pages/AdminUsers";
import AdminTransactions from "../pages/AdminTransactions";
import AdminAuditLogs from "../pages/AdminAuditLogs";
import AdminRoute from "./AdminRoute";


function AppRoutes()
{

    return (

        <Routes>

            <Route
                path="/"
                element={
                    <PublicRoute>
                <Login/>
                </PublicRoute>
            }
            />


                <Route

                    path="/dashboard"

                    element={

                        <ProtectedRoute>

                            <Dashboard />

                        </ProtectedRoute>

                    }

                />

                <Route
                        path="/send-money"
                        element={<SendMoney/>}
                    />

                    <Route 
                        path="/deposit"
                        element={<Deposit />}
                    />

                    <Route
                        path="/withdraw"
                        element={<Withdraw />}
                    />

                    <Route
                        path="/transactions"
                        element={<TransactionHistory />}
                    />

                    <Route 
                    path="/settings"
                    element={<Settings />}
                    />

                    <Route
                        path="/register"
                        element={
                            <PublicRoute>
                        <Register />
                        </PublicRoute>
                    }
                    />

                    <Route
                        path="/forgot-password"
                        element={
                             <PublicRoute>
                        <ForgotPassword />
                        </PublicRoute>
                    }
                    />

                    <Route
                        path="/reset-password"
                        element={<ResetPassword />}
                    />           

                    <Route
                        path="/verify-email"
                        element={<VerifyEmail />}
                    />        
                        <Route

                        path="/admin"

                        element={

                        <AdminRoute>

                        <AdminDashboard/>

                        </AdminRoute>

                        }

                        />    

                        <Route
                            path="/admin/users"
                            element={
                                <AdminRoute>
                                    <ManageUsers/>
                                </AdminRoute>
                            }
                        />    

                        <Route
                        path="/admin/transactions"
                        element={
                            <AdminRoute>
                                <AdminTransactions />
                            </AdminRoute>
                        }
                        />

                        <Route
                            path="/admin/auditlogs"
                            element={
                                <AdminRoute>
                                    <AdminAuditLogs />
                                </AdminRoute>
                            }
                        />

        </Routes>    

    );

}


export default AppRoutes;