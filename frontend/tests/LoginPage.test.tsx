import { describe, test, expect } from 'vitest';
import { render } from 'vitest-browser-react';
import { BrowserRouter } from 'react-router';
import { LoginPage } from '../src/pages/LoginPage';

describe('LoginPage', () => {
    test('performs login successfully', async () => {
        const screen = await render(
            <BrowserRouter>
                <LoginPage />
            </BrowserRouter>
        );
        const userField = screen.getByPlaceholder('Username');
        const passField = screen.getByPlaceholder('Password');

        await userField.fill('username123');
        await passField.fill('password123');

        await screen.getByRole('button', { name: 'Login' }).click();
        await expect.element(screen.getByText('Invalid credentials')).not.toBeInTheDocument();
    });
});