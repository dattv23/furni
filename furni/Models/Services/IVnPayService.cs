﻿using System;
namespace furni.Models.Services
{
	public interface IVnPayService
	{
		string CreatePaymentUrl(HttpContext context, VnPayRequestModel model);
		VnPaymentResponseModel PaymentExcute(IQueryCollection collections);
	}
}

