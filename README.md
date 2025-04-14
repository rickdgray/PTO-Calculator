# PTO Accrual Calculator

This tool generates a calendar file (`PTO Accrual.ics`) for PTO (Paid Time Off) accrual over exactly one year starting on January first of the current year, based on a specified accrual rate. The accrual rate is adjusted by how much PTO you want reserved for future plans such as the holidays. The generated calendar file can be used to track PTO accrual over time, so you do not overuse your PTO throughout the year.

## Usage
Your own PTO accrual rate must be given, and reserved days defaults to 0.

Examples:
```bash
calc-pto --days-earned-per-year 10 --reserved-days 0
calc-pto --days-earned-per-year 20
calc-pto ---earned 10 --reserved 5
calc-pto -e 10 -r 5
calc-pto -e 20
```
