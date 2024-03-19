import { formatDate } from "./formatters";

describe("Date formatter", () => {
  it("should format Thu Dec 07 2023 01:00:00 GMT+0100 (Central European Standard Time) to 2023-12-07", () => {
    const date = new Date(
      "Thu Dec 07 2023 01:00:00 GMT+0100 (Central European Standard Time)",
    );

    expect(formatDate(date)).toBe("2023-12-07");
  });

  it("should format Thu Dec 06 2023 23:50:00 GMT+0100 (Central European Standard Time) to 2023-12-06", () => {
    const date = new Date(
      "Thu Dec 06 2023 01:00:00 GMT+0100 (Central European Standard Time)",
    );

    expect(formatDate(date)).toBe("2023-12-06");
  });
});
