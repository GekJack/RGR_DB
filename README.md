# ��������� ������� ���� �����, ����������� �� ������� � ���� PostgreSQL


### ��������� ���� �����
# ![File](file.png)

## ������ ����� ������� ����� "Model"
1. [Model](#1)
2. [CloseConnection](#2)
3. [AddDataToTableModel](#3)
4. [GetColumnTypes](#4)
5. [GetPrimaryKeyColumn](#5)
6. [GetConvertedValues](#6)
7. [UpdateDataInTable](#7)
8. [GetColumnNameOfTable](#8)
9. [GetRowsOfTable](#9)
10. [GetAllTables](#10)
11. [DeleteDataOfTable](#11)
12. [GenerateDataToCurrentTable](#12)
13. [SearchFirst](#13)
14. [SearchSecond](#14)
15. [SearchThird](#15)

### <a id="1">1.Model()</a>
#### ����������� ����� Model, ���� �������� ���������� �� ���� �����.
### <a id="2">2.CloseConnection()</a>
#### ����� ������ Model, ���� ������� �'������� � ����� �����, ���������� �� �������� ���������� �� ������� ����.
### <a id="3">3.AddDataToTableModel(List<(string Column, string Value)> values,string table_name)</a>
#### ����� ������ Model, ���� ���� ��� ��� ������ ������������ � �������� �������, ������ �� ��������� ������ ������� � ����� �������� �� ������ ��������, � ����� ��'� �������.
### <a id="4">4.GetColumnTypes(string tableName)</a>
#### ����� ����� Model, ���� ������ � information_schema ���� �������� �������, ��'� ��� ���������� � ����� ��������� ������, ������� ������� � ����:����� ��������, ��������:���.
### <a id="5">5.GetPrimaryKeyColumn(string table_name)</a>
#### ����� ����� Model, ���� ������ � information_schema ����� �������� �������, ���� � primary key.
### <a id="6">6.GetConvertedValues(Dictionary<string, string> columnTypes, string column, string value)</a>
#### ����� ����� Model, ���� �������� �������� ������ ������������ � ���� ��� �������� � ���� �����.������ � ����� ���������: ������� ���� �������� � �� ����, ����� �������� � ��������(string),��� ����� ������������.
### <a id="7">7.UpdateDataInTable(List<(string Column, string Value)> values_res, string table_name, int pk)</a>
#### ����� ����� Model, ���� ������� ��� ���������� �� *pk* ����� ������ �������. ���������: ������ ������� � ���� �������� �� ���������� ������, ����� �������, ��������� ����.
### <a id="8">8.GetColumnNameOfTable(string table_name)</a>
#### ����� ����� Model, ���� ������ ����� �������� �������� ������� � information_schema.
### <a id="9">9.GetRowsOfTable(string table_name, int page_num)</a>
#### ����� ����� Model, ���� ������ ������ �����������(50 ������) � �������� �������, ��� ������.���������:����� �������, ����� �������.
### <a id="10">10.GetAllTables()</a>
#### ����� ����� Model, ���� ������ ����� ��� ������� � ��� ����� � information_schema.
### <a id="11">11.DeleteDataOfTable(string table_name, int pk, string pk_str)</a>
#### ����� ����� Model, ���� ������� ��������� ����� � ������� �� *pk*.���������: ����� �������, �������� *pk*,����� ��������,���� � *pk*. 
### <a id="12">12.GenerateDataToCurrentTable(string proc_name, int count_rows)</a>
#### ����� ����� Model, ���� ������� ��������� ��� ��������� ���������� ������ � �������.���������: ����� ���������, ������� ������ ��� ����������.
### <a id="13">13.SearchFirst(string status)</a>
#### ����� ����� Model, ���� ������� ������ SQL-����� ��� ������.��������: ������ ����������, �� ���� ��������� �����(true or false).
### <a id="14">14.SearchSecond(string start_time)</a>
#### ����� ����� Model, ���� ������� ������ SQL-����� ��� ������. ��������: ���(������� ���������) �� ����� ����������� �����.
### <a id="15">15.SearchThird(string city, string exp_years)</a>
#### ����� ����� Model, ���� ������� ����� SQL-����� ��� ������. ��������: ����� ����, ������� ������ ������ ��������.











