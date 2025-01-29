using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Protocol;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class OrderItemsControllerTests
    {
        private readonly Mock<IOrderItemService> _orderItemServiceMock;
        private readonly OrderItemsController _controller;

        public OrderItemsControllerTests()
        {
            _orderItemServiceMock = new Mock<IOrderItemService>();
            _controller = new OrderItemsController(_orderItemServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_viewresult()
        {
            // Arrange
            var products = new List<Product>()
            {
                new Product(){ Id = 1, Name="Product1"},
                new Product(){ Id = 2, Name="Product2"}
            };

            var data = new PagedResult<OrderItem> {
                Results = new List<OrderItem>() {
                    new OrderItem(){ Id= 1},
                    new OrderItem(){ Id= 2},
                } 
            };
            // Act
            _orderItemServiceMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(products).Verifiable();
            _orderItemServiceMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(data).Verifiable();
            var result = await _controller.Index() as ViewResult;
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(data, result.Model);
            Assert.Equal(_controller.ViewBag.Products, products);
            _orderItemServiceMock.VerifyAll();

        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_null()
        {
            // Arrange
            int? id = null;
            // Act
            var result = await _controller.Details(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_orderitem_is_null()
        {
            // Arrange
            int id = 1;
            // Act
            _orderItemServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((OrderItem)null).Verifiable();
            var result = await _controller.Details(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _orderItemServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Details_should_return_viewresult()
        {
            // Arrange
            int id = 1;
            OrderItem orderItem = new OrderItem() { Id = id };
            // Act
            _orderItemServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(orderItem).Verifiable();
            var result = await _controller.Details(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            _orderItemServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Create_should_return_RedirectToAction_when_modalstate_is_not_valid()
        {
            // Arrange
            OrderItem orderItem = new OrderItem() { };
            // Act
            _orderItemServiceMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(It.IsAny<IList<Product>>()).Verifiable();
            _orderItemServiceMock.Setup(x => x.Save(It.IsAny<OrderItem>())).Returns(Task.CompletedTask).Verifiable();
            var result = await _controller.Create(orderItem);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Create_should_return_viewresult_when_modalstate_is_not_valid()
        {
            // Arrange
            OrderItem orderitem = new OrderItem() { };
            var order_data = new List<Order>()
            {
                new Order() { Id = 1},
                new Order() { Id = 2}
            };

            var products_data = new List<Product>()
            {
                new Product() { Id = 1},
                new Product() { Id = 2}
            };

            // Act
            _orderItemServiceMock.Setup(x => x.GetOrdersAsync()).ReturnsAsync(order_data).Verifiable();                                          
            _orderItemServiceMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(products_data).Verifiable();
            _controller.ModelState.AddModelError("Key", "Error");
            var result = await _controller.Create(orderitem) as ViewResult;
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(
                new SelectList(order_data, "Id", "Id")
                .Select(x => x.Value).ToList(), (_controller.ViewBag.Orders as SelectList).Select(x => x.Value).ToList());
            Assert.Equal(new SelectList(products_data, "Id", "Name").Select(x => x.Value).ToList(), (_controller.ViewBag.Products as SelectList).Select(x => x.Value).ToList());  
            _orderItemServiceMock.VerifyAll();
            
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_null()
        {
            // Arrange
            int? id = null;
            // Act
            var result = await _controller.Edit(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_orderitem_is_null()
        {
            // Arrange
            int id = 1;
            // Act
            _orderItemServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((OrderItem)null).Verifiable();
            var result = await _controller.Edit(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _orderItemServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_Return_viewresult()
        {
            // Arrange
            int id = 1;
            OrderItem orderitem = new OrderItem() { Id = id };

            var products_data = new List<Product>
            {
                new Product() { Id = 1, Name = "Nimi 1" },
                new Product() { Id = 2, Name = "Nimi 2" }
            };

            var orders_data = new List<Order>
            {
                new Order() { Id = 1 },
                new Order() { Id = 2 }
            };
            // Act
            _orderItemServiceMock.Setup(x => x.GetOrdersAsync()).ReturnsAsync(orders_data).Verifiable();
            _orderItemServiceMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(products_data).Verifiable();
            _orderItemServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(orderitem).Verifiable();
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(orderitem, result.Model);
            Assert.Equal(new SelectList(products_data, "Id", "Name").Select(x => x.Value).ToList(), (_controller.ViewBag.Products as SelectList).Select(x => x.Value).ToList());
            Assert.Equal(new SelectList(orders_data, "Id", "Id", orderitem.OrderId).Select(x => x.Value).ToList(), (_controller.ViewBag.OrderId as SelectList).Select(x => x.Value).ToList());
            _orderItemServiceMock.VerifyAll();
        }



        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_null()
        {
            // Arrange
            int? id = null;
            // Act
            var result = await _controller.Delete(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_orderitem_is_null()
        {
            // Arrange
            int id = 1;
            // Act
            _orderItemServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((OrderItem)null).Verifiable();
            var result = await _controller.Delete(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _orderItemServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_return_viewresult()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = id };
            // Act
            _orderItemServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(orderItem).Verifiable();
            var result = await _controller.Delete(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(orderItem, (result as ViewResult).Model);
            _orderItemServiceMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index_when_orderitem_exists()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = id };
            // Act
            _orderItemServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(orderItem).Verifiable();
            _orderItemServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.CompletedTask).Verifiable();
            var result = await _controller.DeleteConfirmed(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), (result as RedirectToActionResult).ActionName);
            _orderItemServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_post_should_return_notfound_when_id_mismatch()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = 2 }; // Different ID
            // Act
            var result = await _controller.Edit(id, orderItem);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_post_should_redirect_to_index_on_successful_save()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = id };
            // Act
            _orderItemServiceMock.Setup(x => x.Save(It.IsAny<OrderItem>())).Returns(Task.CompletedTask).Verifiable();
            var result = await _controller.Edit(id, orderItem);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), (result as RedirectToActionResult).ActionName);
            _orderItemServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_post_should_return_viewresult_when_modelstate_is_invalid()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = id };

            var products_data = new List<Product>
            {
                new Product() { Id = 1, Name = "Nimi 1" },
                new Product() { Id = 2, Name = "Nimi 2" }
            };

            var orders_data = new List<Order>
            {
                new Order() { Id = 1 },
                new Order() { Id = 2 }
            };

            _controller.ModelState.AddModelError("Key", "Error");
            // Act
            _orderItemServiceMock.Setup(x => x.GetOrdersAsync()).ReturnsAsync(orders_data).Verifiable();
            _orderItemServiceMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(products_data).Verifiable();
            var result = await _controller.Edit(id, orderItem) as ViewResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderItem, result.Model);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(new SelectList(products_data, "Id", "Name").Select(x => x.Value).ToList(), (_controller.ViewBag.Products as SelectList).Select(x => x.Value).ToList());
            Assert.Equal(new SelectList(orders_data, "Id", "Id", orderItem.OrderId).Select(x => x.Value).ToList(), (_controller.ViewBag.OrderId as SelectList).Select(x => x.Value).ToList());

        }

        [Fact]
        public async Task Create_should_return_view_when_modelstate_is_invalid()
        {
            // Arrange
            var orderItem = new OrderItem();
            _controller.ModelState.AddModelError("Key", "Error");
            var products = new List<Product> { new Product { Id = 1, Name = "Product1" } };
            var orders = new List<Order> { new Order { Id = 1 } };
            // Act
            _orderItemServiceMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(products).Verifiable();
            _orderItemServiceMock.Setup(x => x.GetOrdersAsync()).ReturnsAsync(orders).Verifiable();
            var result = await _controller.Create(orderItem) as ViewResult;
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(new SelectList(products, "Id", "Name").Select(x => x.Value).ToList(), (_controller.ViewBag.Products as SelectList).Select(x => x.Value).ToList());
            Assert.Equal(new SelectList(orders, "Id", "Id").Select(x => x.Value).ToList(), (_controller.ViewBag.Orders as SelectList).Select(x => x.Value).ToList());
            _orderItemServiceMock.VerifyAll();
        }

        [Fact]
        public async Task OrderItemExists_should_return_true_when_item_exists()
        {
            // Arrange
            int id = 1;
            _orderItemServiceMock.Setup(x => x.Includes(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.OrderItemExists(id);

            // Assert
            Assert.True(result);
            _orderItemServiceMock.Verify(x => x.Includes(id), Times.Once);
        }

        [Fact]
        public async Task Edit_post_should_throw_exception_when_concurrency_exception_and_item_exists()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = id };
            _orderItemServiceMock.Setup(x => x.Save(orderItem)).Throws<DbUpdateConcurrencyException>();
            _orderItemServiceMock.Setup(x => x.Includes(id)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _controller.Edit(id, orderItem));
            _orderItemServiceMock.Verify(x => x.Save(orderItem), Times.Once);
            _orderItemServiceMock.Verify(x => x.Includes(id), Times.Once);
        }


        [Fact]
        public async Task Create_should_return_view_with_products_and_orders()
        {
            // Arrange
            var products = new List<Product>
    {
        new Product { Id = 1, Name = "Product1" },
        new Product { Id = 2, Name = "Product2" }
    };

            var orders = new List<Order>
    {
        new Order { Id = 1 },
        new Order { Id = 2 }
    };

            _orderItemServiceMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(products);
            _orderItemServiceMock.Setup(x => x.GetOrdersAsync()).ReturnsAsync(orders);

            // Act
            var result = await _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(new SelectList(products, "Id", "Name").Select(x => x.Value).ToList(), (_controller.ViewBag.Products as SelectList).Select(x => x.Value).ToList());
            Assert.Equal(new SelectList(orders, "Id", "Id").Select(x => x.Value).ToList(), (_controller.ViewBag.Orders as SelectList).Select(x => x.Value).ToList());
            _orderItemServiceMock.Verify(x => x.GetProductsAsync(), Times.Once);
            _orderItemServiceMock.Verify(x => x.GetOrdersAsync(), Times.Once);
        }

        [Fact]
        public async Task Edit_post_should_return_notfound_when_concurrency_exception_and_item_does_not_exist()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = id };
            _orderItemServiceMock.Setup(x => x.Save(orderItem)).Throws<DbUpdateConcurrencyException>();
            _orderItemServiceMock.Setup(x => x.Includes(id)).ReturnsAsync(false);

            // Act
            var result = await _controller.Edit(id, orderItem);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _orderItemServiceMock.Verify(x => x.Save(orderItem), Times.Once);
            _orderItemServiceMock.Verify(x => x.Includes(id), Times.Once);
        }

    }


}

