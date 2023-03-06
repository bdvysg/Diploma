import pyodbc 
import csv
conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=BDVYSG\MSSQLSERVER01;'
                      'Database=Market;'
                      'Trusted_Connection=yes;')
 
cursor = conn.cursor()

with open('C:/base/stud/actual/Supermarket CRM/SQL/Insert/Data.csv') as csvfile:
    reader = csv.reader(csvfile, delimiter=';')
    for row in reader:
        sql = f"INSERT INTO Product(Pr_BarCode, Pr_Description, Pr_Brand, Pr_Category, Pr_PriceOpt, Pr_Price, Pr_Qty, Pr_Reorder, Pr_Title, Pr_ImgUrl) VALUES (NULL, '', IsNull((SELECT TOP 1 Br_Id FROM Brand WHERE Br_Title = '{row[1]}'), 54), {row[5]}, {row[2].replace(',', '.')}, {row[2].replace(',', '.')}, '{row[4]}', NULL, '{row[0]}', '{row[3]}')"
        print(sql)
        cursor.execute(sql)
        conn.commit()
        #print(row[2])


cursor.close()
conn.close()