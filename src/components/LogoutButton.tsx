import { useNavigate } from "react-router";
import { handleLogout } from "../api/auth";

export function LogoutButton() {
    const navigate = useNavigate();

    const onLogout = () => {
        handleLogout();
        navigate('/login', { replace: true });
    };

    return (
        <button
            className="px-6 py-2 border border-red-500 text-red-500 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-lg transition-colors cursor-pointer font-medium focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2 dark:focus:ring-offset-gray-900"
            onClick={onLogout}
        >
            Log out
        </button>
    );
}
