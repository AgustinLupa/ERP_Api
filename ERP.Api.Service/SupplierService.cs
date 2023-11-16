﻿using Dapper;
using ERP.Api.Entity.Contracts;
using MySql.Data.MySqlClient;
using SystemERP.Model;

namespace ERP.Api.Service;

public class SupplierService : ISupplierService
{
    private readonly Context _context;

    public SupplierService(Context context)
    {
        _context = context;
    }

    public async Task<int> CreateSupplier(Supplier supplier)
    {
        using (var connection = _context.CreateConnection())
        {
            try
            {
                connection.Open();
                var mysql = @"INSERT INTO supplier(name, adress, phone) 
                                  Values (@Name, @Adress, @Phone);
                                  SELECT CAST(SCOPE_IDENTITY() as int)";
                var result = await connection.QuerySingleAsync<int>(mysql, supplier);
                connection.Close();
                return result;
            }
            catch (Exception)
            {
                connection.Close();
                return 0;
            }
        }
    }

    public async Task<bool> UpdateSupplier(Supplier supplier)
    {
        using (var connection = _context.CreateConnection())
        {
            try
            {
                connection.Open();
                var mysql = @"UPDATE supplier SET name = @Name, adress = @Adress, phone = @Phone, state= @state WHERE (id =@Id)";
                var result = await connection.ExecuteAsync(mysql, supplier);
                connection.Close();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                connection.Close();
                return false;
            }
        }
    }

    public async Task<bool> DeleteSupplier(Supplier supplier)
    {
        using (var connection = _context.CreateConnection())
        {
            try
            {
                connection.Open();
                var mysql = @"UPDATE supplier SET state= 0 WHERE (id =@Id)";
                var result = await connection.ExecuteAsync(mysql, supplier);
                connection.Close();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                connection.Close();
                return false;
            }
        }
    }

    public async Task<IEnumerable<Supplier>> GetAll()
    {

        using (var connection = _context.CreateConnection())
        {
            try
            {
                var query = @"
                    SELECT s.id, s.name, s.state, s.adress, s.phone                   
                    FROM supplier s";
                var result = await connection.QueryAsync<Supplier>(query);
                return result;
            }
            catch (Exception)
            {
                List<Supplier> supplier = new List<Supplier>();
                return supplier;
            }
        }

    }

    public async Task<Supplier> GetById(int id)
    {
        using (var connection = _context.CreateConnection())
        {
            try
            {
                connection.Open();
                var query = @"
                    SELECT s.id, s.name, s.state, s.adress, s.phone                    
                    FROM supplier s
                    WHERE (s.id = @Id)";
                var result = await connection.QueryFirstOrDefaultAsync<Supplier>(query);                    
                return result;
            }
            catch (Exception)
            {
                Supplier supplier = new Supplier();
                return supplier;
            }
        }
    }

    public async Task<IEnumerable<Supplier>> GetActiveSupplier()
    {
        using (var connection = _context.CreateConnection())
        {
            try
            {

                var query = @"
                    SELECT s.id, s.name, s.state, s.adress, s.phone                    
                    FROM supplier s
                    WHERE (s.state = 1)";
                var result = await connection.QueryAsync<Supplier>(query);
                return result;
            }
            catch (Exception)
            {
                List<Supplier> supplier = new List<Supplier>();
                return supplier;
            }
        }
    }
}

