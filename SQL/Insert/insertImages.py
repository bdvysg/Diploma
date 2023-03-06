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
        sql = f"UPDATE Product SET Pr_Image = (SELECT * FROM OPENROWSET(BULK N'C:/base/stud/actual/Supermarket CRM/SQL/Insert/images/{row[3].split('/')[-1]}', SINGLE_BLOB) as img) WHERE Pr_ImgUrl = '{row[3]}'"
        print(sql)
        cursor.execute(sql)
        conn.commit()
        #print(row[2])


cursor.close()
conn.close()