import { describe, expect, test } from "vitest";
import { render } from 'vitest-browser-react';
import { LogoutButton } from "../src/components/LogoutButton";
import { BrowserRouter, Route, Routes } from "react-router";



describe('Logout button', () => {
    test('Clears the auth token when clicked', async () => {
        localStorage.clear();
        localStorage.setItem('authToken', 'test-token');
        const screen = await render(
            <BrowserRouter >
                <Routes>
                    <Route path='/' element={<LogoutButton />}></Route>
                    <Route path='/login' element={null}></Route>
                </Routes>
            </BrowserRouter >
        );
        const button = screen.getByText("Log out", { exact: true });
        await expect.element(button).toBeInTheDocument();

        await expect.poll(() => localStorage.getItem("authToken")).not.toBeNull();
        await button.click();
        await expect.poll(() => localStorage.getItem("authToken")).toBeNull();
    });
});