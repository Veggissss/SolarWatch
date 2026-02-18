import { BrowserRouter } from "react-router";
import { describe, expect, test } from "vitest";
import { render } from "vitest-browser-react";
import { SolarWatchPage } from "../src/pages/SolarWatchPage"

describe('SolarWatchPage', () => {

    test('performs error on empty search', async () => {
        localStorage.setItem('authToken', 'test-token');

        const screen = await render(
            <BrowserRouter>
                <SolarWatchPage />
            </BrowserRouter>
        );
        const submitButton = screen.getByText("Search");
        await submitButton.click()

        await expect.element(screen.getByText("At least give me a city bro!", { exact: false })).toBeInTheDocument();
    })

    test('performs search sucessfully', async () => {
        localStorage.setItem('authToken', 'test-token');

        const screen = await render(
            <BrowserRouter>
                <SolarWatchPage />
            </BrowserRouter>
        );
        const citySearchField = screen.getByPlaceholder("Enter city name...");
        await citySearchField.fill("Lon");

        // Auto suggestion
        await expect.element(screen.getByText("London", { exact: true })).toBeInTheDocument();

        // No info before search
        await expect.element(screen.getByText("Sunset", { exact: true })).not.toBeInTheDocument();
        await expect.element(screen.getByText("Sunrise", { exact: true })).not.toBeInTheDocument();

        const submitButton = screen.getByText("Search");
        await submitButton.click()

        // Result
        await expect.element(screen.getByText("Sunset")).toBeInTheDocument();
        await expect.element(screen.getByText("Sunrise")).toBeInTheDocument();
    })
})