﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Model;
using GK.Model.Parms;
using Dapper;
using GK.DAO;
using System.Data;
namespace GK.Service.Report
{
    public class CruisesReport
    {
        public CruisesReport()
        {

        }
        /// <summary>
        /// 邮船收入时间段统计
        /// </summary>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> CruisesIncome(string ksrq, string jsrq)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select ta.*,tb.name from ");
            sql.Append("(");
            sql.AppendFormat("select cruisesno,sum(CURR0) as total_amount from MENUBILLH where convert(date,notedate) between '{0}' and '{1}'  group by CRUISESNO ", ksrq, jsrq);
            sql.Append(" ) ta,CRUISES tb");
            sql.Append(" where ta.CRUISESNO = tb.CODE");

            using (GoldKey_DB db = new GoldKey_DB())
            {
                return db.Get_Con.Query(sql.ToString());
            }
        }
        /// <summary>
        /// 航次收入统计
        /// </summary>
        /// <param name="rcno"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> RcIncome(string rcno)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT t1.*, \n");
            sql.Append("t2.name, \n");
            sql.Append("(SELECT TYPENAME FROM dbo.MENUTYPE WHERE TYPENO=t1.TYPENO) typename \n");
            sql.Append(" FROM \n");
            sql.Append("( \n");
            sql.Append("SELECT tb.rcno,ta.placeno,ta.placename,ta.typeno,ta.cruisesno,SUM(ta.CURR) AS curr FROM dbo.MENUBILLM ta,dbo.MENUBILLH tb \n");
            sql.Append("WHERE ta.NOTENO = tb.NOTENO \n");
            sql.Append("GROUP BY tb.RCNO,ta.PLACENO,ta.PLACENAME,ta.TYPENO,ta.CRUISESNO \n");
            sql.Append(") t1,dbo.CRUISES t2 \n");
            sql.Append("WHERE \n");
            sql.Append("t1.CRUISESNO=t2.CODE \n");
            if (!string.IsNullOrEmpty(rcno))
            {
                sql.AppendFormat("and t1.RCNO='{0}' \n", rcno);
            }
            sql.Append("ORDER BY t1.TYPENO ASC");
            using (GoldKey_DB db = new GoldKey_DB())
            {
                return db.Get_Con.Query(sql.ToString());
            }

        }
        /// <summary>
        /// 邮轮各航次销售统计
        /// </summary>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> CruisesRc(string ksrq, string jsrq,string cruisesno)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT cruisesno,rcno,(SELECT COUNT(GUESTID) AS cnt FROM dbo.GUEST WHERE RCNO=ta.rcno) as guestcnt,SUM(TCURR0) AS curr FROM dbo.MENUBILLH ta \n");
            sql.AppendFormat("WHERE CONVERT(DATE,ta.NOTEDATE) BETWEEN '{0}' AND '{1}' \n",ksrq,jsrq);
            if(!string.IsNullOrEmpty(cruisesno))
            { 
                sql.AppendFormat(" and ta.cruisesno='{0}' ", cruisesno);
            }
            sql.Append("GROUP BY CRUISESNO,RCNO");
            using (GoldKey_DB db = new GoldKey_DB())
            {
                return db.Get_Con.Query(sql.ToString());
            }
        }
        /// <summary>
        /// 航次销售统计
        /// </summary>
        /// <param name="rcno"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Cruises_RcSale_Detail(string rcno)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ta.placename,ta.placeno,ta.typeno, \n");
            sql.Append("       ta.menuname, \n");
            sql.Append("       (SELECT TYPENAME \n");
            sql.Append("        FROM   dbo.MENUTYPE \n");
            sql.Append("        WHERE  TYPENO = ta.TYPENO) AS typename, \n");
            sql.Append("       ta.price, \n");
            sql.Append("       ta.amount, \n");
            sql.Append("       ta.curr, \n");
            sql.Append("       tc.name, \n");
            sql.Append("       tc.surname, \n");
            sql.Append("       tc.enname, \n");
            sql.Append("       tc.address, \n");
            sql.Append("       tc.sex, \n");
            sql.Append("       tc.birthdate \n");
            sql.Append("FROM   dbo.MENUBILLM ta, \n");
            sql.Append("       dbo.MENUBILLH tb, \n");
            sql.Append("       dbo.GUEST tc \n");
            sql.Append("WHERE  ta.NOTENO = tb.NOTENO \n");
            sql.Append("       AND ta.GUESTID = tc.GUESTID \n");
            sql.AppendFormat("       AND tb.RCNO = '{0}' order by ta.placeno asc,ta.typeno asc",rcno);
            using (GoldKey_DB db = new GoldKey_DB())
            {
                return db.Get_Con.Query(sql.ToString());
            }

        }
        /// <summary>
        /// 邮轮航次，分类消费
        /// </summary>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Cruises_Rc_Class(string ksrq, string jsrq,string cruisesno="")
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT t1.cruisesno, \n");
            sql.Append("       t1.cruisesname, \n");
            sql.Append("       t1.rcno, \n");
            sql.Append("       t1.placeno, \n");
            sql.Append("       t1.placename, \n");
            sql.Append("       t1.typeno, \n");
            sql.Append("       t1.typename, \n");
            sql.Append("       Sum(t1.curr) AS je \n");
            sql.Append("FROM   (SELECT cruisesno, \n");
            sql.Append("               (SELECT TOP 1 NAME \n");
            sql.Append("                FROM   dbo.CRUISES \n");
            sql.Append("                WHERE  CODE = ta.cruisesno)          AS cruisesname, \n");
            sql.Append("               typeno, \n");
            sql.Append("               (SELECT TOP 1 TYPENAME \n");
            sql.Append("                FROM   dbo.MENUTYPE \n");
            sql.Append("                WHERE  TYPENO = ta.typeno)           AS typename, \n");
            sql.Append("               placeno, \n");
            sql.Append("               (SELECT TOP 1 PLACENAME \n");
            sql.Append("                FROM   dbo.MENUPLACE \n");
            sql.Append("                WHERE  PLACENO = ta.placeno \n");
            sql.Append("                       AND CRUISESNO = ta.cruisesno) AS placename, \n");
            sql.Append("               curr, \n");
            sql.Append("               (SELECT TOP 1 RCNO \n");
            sql.Append("                FROM   dbo.MENUBILLH \n");
            sql.Append("                WHERE  NOTENO = ta.noteno)           AS rcno \n");
            sql.Append("        FROM   dbo.MENUBILLM ta \n");
            sql.AppendFormat("        WHERE  CONVERT(DATE, ta.DATE0) BETWEEN '{0}' AND '{1}' \n",ksrq,jsrq);
            if (!string.IsNullOrEmpty(cruisesno))
            {
                sql.AppendFormat(" and ta.cruisesno = '{0}' \n", cruisesno);
            }
            sql.Append(" ) t1 \n");
            sql.Append(" GROUP  BY t1.cruisesno, \n");
            sql.Append("          t1.cruisesname, \n");
            sql.Append("          t1.typeno, \n");
            sql.Append("          t1.typename, \n");
            sql.Append("          t1.placeno, \n");
            sql.Append("          t1.placename, \n");
            sql.Append("          t1.rcno \n");
            sql.Append("ORDER  BY t1.CRUISESNO, \n");
            sql.Append("          t1.rcno, \n");
            sql.Append("          t1.PLACENO, \n");
            sql.Append("          t1.TYPENO");
            using (GoldKey_DB db = new GoldKey_DB())
            {
                return db.Get_Con.Query(sql.ToString());
            }
        }
        /// <summary>
        /// 消费项目排行
        /// </summary>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <param name="cruisesno"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> MenuCodeSaleTop(string ksrq,string jsrq,string cruisesno="")
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ta.menuno, \n");
            sql.Append("       ta.menuname, \n");
            sql.Append("       ta.je \n");
            sql.Append("FROM   (SELECT MENUNO, \n");
            sql.Append("               t1.MENUNAME, \n");
            sql.Append("               Sum(curr) AS je \n");
            sql.Append("        FROM   dbo.MENUBILLM t1 \n");
            sql.AppendFormat("        WHERE  CONVERT(DATE, t1.DATE0) BETWEEN '{0}' AND '{1}' \n",ksrq,jsrq);
            if(!string.IsNullOrEmpty(cruisesno))
            {
                sql.AppendFormat(" and t1.cruisesno='{0}' \n", cruisesno);
            }
            sql.Append("        GROUP  BY t1.MENUNO, \n");
            sql.Append("                  t1.MENUNAME) ta \n");
            sql.Append("ORDER  BY ta.je DESC");
            using (GoldKey_DB db = new GoldKey_DB())
            {
                return db.Get_Con.Query(sql.ToString());
            }
        }
        /// <summary>
        /// 航次销售排行
        /// </summary>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <param name="cruisesno"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> RcSaleTop(string ksrq,string jsrq,string cruisesno="")
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT t1.rcno, \n");
            sql.Append("t1.je, \n");
            sql.Append("convert(nvarchar(10),t2.bdate,120) as bdate, \n");
            sql.Append("convert(nvarchar(10),t2.edate,120) as edate, \n");
            sql.Append("t2.no, \n");
            sql.Append("t2.scgkname, \n");
            sql.Append("t2.lcgkname, \n");
            sql.Append("(SELECT name FROM dbo.CRUISES WHERE CODE = t2.CRUISESNO) AS name \n");
            sql.Append(" FROM \n");
            sql.Append("(SELECT RCNO, \n");
            sql.Append("       Sum(CURR0) AS je \n");
            sql.Append("FROM   dbo.MENUBILLH \n");
            sql.AppendFormat("WHERE  CONVERT(DATE, NOTEDATE) BETWEEN '{0}' AND '{1}' \n",ksrq,jsrq);
            if(!string.IsNullOrEmpty(cruisesno))
            {
                sql.AppendFormat("AND CRUISESNO='{0}' \n",cruisesno);
            }
            sql.Append("GROUP  BY rcno \n");
            sql.Append(") t1,dbo.SHIPCLASS t2 \n");
            sql.Append("WHERE t1.RCNO = t2.RCNO \n");
            sql.Append(" \n");
            sql.Append(" ORDER BY t1.je DESC");

            using (GoldKey_DB db = new GoldKey_DB())
            {
                return db.Get_Con.Query(sql.ToString());
            }
        }
        /// <summary>
        /// 销售站点销售排行
        /// </summary>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <param name="cruisesno"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> PlaceSaleTop(string ksrq, string jsrq, string cruisesno = "")
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT t1.*, \n");
            sql.Append("       (SELECT TOP 1 placename \n");
            sql.Append("        FROM   dbo.MENUPLACE \n");
            sql.Append("        WHERE  PLACENO = t1.PLACENO) AS placename \n");
            sql.Append("FROM   (SELECT placeno, \n");
            sql.Append("               Sum(CURR0) AS je \n");
            sql.Append("        FROM   dbo.MENUBILLH \n");
            sql.AppendFormat("        WHERE  CONVERT(DATE, NOTEDATE) BETWEEN '{0}' AND '{1}' \n",ksrq,jsrq);
            if(!string.IsNullOrEmpty(cruisesno))
            {
                sql.AppendFormat("               AND CRUISESNO = '{0}' \n",cruisesno);
            }
            sql.Append("        GROUP  BY placeno) t1 order by t1.je desc");
            using (GoldKey_DB db = new GoldKey_DB())
            {
                return db.Get_Con.Query(sql.ToString());
            }
        }

    }
}
