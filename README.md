# AccountAPP

一個以 WinForms 製作的簡易記帳工具，資料儲存在本機 SQLite，適合個人離線使用。

## 專案狀態

此專案目前為可用、可維護的封存版本（maintenance mode）。

## 主要功能

- 單筆記帳：新增帳目、類別、金額、記帳日期
- 月份檢視：依 `yyyy-MM` 查詢當月記錄
- 刪除資料：可刪除目前選取的記帳項目
- 自訂類別：可新增收入/支出類別
- 定期項目：支援每天/每月/每年自動入帳
- Excel 匯入：可批次匯入 `.xlsx`
- 錯誤與事件記錄：JSON-lines logger，便於除錯

## 技術資訊

- UI: WinForms
- Target Framework: `net9.0-windows`
- Database: `System.Data.SQLite`
- Excel: `EPPlus 4.5.3.3`

## 執行環境

- Windows
- .NET SDK 9.0+

## 開發與啟動

在專案根目錄執行：

```powershell
dotnet restore
dotnet build -c Debug
dotnet run
```

輸出檔案位置：

- `bin/Debug/net9.0-windows/`

## 資料與檔案位置

- SQLite DB: `DB/Account.db`
- Logger 輸出: `logs/yyyy-MM-dd.log`

## 資料表概覽

- `Account(AccountName, Type, AccountValue, DATE)`
- `Deposit(AccountName, Type, AccountValue, DATE)`
- `Type(Type, TypeClass)`
- `Schedule(Id, Name, Type, Amount, Frequency, LastApplied, Enabled)`

## Excel 匯入格式

工作表第 1 列為欄位名稱，第 2 列起為資料：

1. 帳目
2. 類別
3. 金額
4. 日期

日期可接受：

- `yyyy-MM-dd`
- `yyyy-MM-dd-ddd`

匯入時會略過不完整或格式錯誤的列，並顯示成功/略過筆數。

## 定期項目規則

- `daily`: 當天尚未套用則執行
- `monthly`: 本月尚未套用則執行
- `yearly`: 本年尚未套用則執行

啟動程式時會自動檢查並套用到期項目。

## 注意事項

- 這是桌面端本機工具，未做多使用者併發設計
- `DATE` 欄位目前為文字格式儲存（相容舊資料）
- 若後續要長期維護，建議補上單元測試與 DB migration 流程
