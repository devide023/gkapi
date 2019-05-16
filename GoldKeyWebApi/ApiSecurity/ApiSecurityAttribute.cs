using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using System.Net.Http;
using System.Net;
namespace GoldKeyWebApi.ApiSecurity
{
    public class ApiSecurityAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //从http请求的头里面获取身份验证信息，验证是否是请求发起方的ticket
            var authorization = actionContext.Request.Headers.Authorization;
            if ((authorization != null) && (authorization.Scheme != null))
            {
                //解密用户ticket,并校验用户名密码是否匹配
                var encryptTicket = authorization.Scheme;
                if (ValidateTicket(encryptTicket))
                {
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
            else
            {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous) base.OnAuthorization(actionContext);
                else HandleUnauthorizedRequest(actionContext);
            }

        }

        //校验用户名密码（正式环境中应该是数据库校验）
        private bool ValidateTicket(string encryptTicket)
        {
            try
            {
                //解密Ticket
                var strTicket = FormsAuthentication.Decrypt(encryptTicket).UserData;

                //从Ticket里面获取用户名和密码
                var index = strTicket.IndexOf("&");
                string strUser = strTicket.Substring(0, index);
                string strPwd = strTicket.Substring(index + 1);
                //Api.Service.UserService us = new Api.Service.UserService();
                //var list = us.check_userlogin(strUser, strPwd);
                //if (list.Count() > 0)
                //{
                    return true;
                //}
                //else
                //{
                //    return false;
                //}
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}