using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NovelPlatform.Models;

namespace NovelPlatform.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure DB created
            await context.Database.EnsureCreatedAsync();

            // 1. Seed Roles
            string[] roles = new[] { "Admin", "Reader" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Seed Users
            ApplicationUser adminUser = null!;
            ApplicationUser readerUser = null!;
            ApplicationUser saraUser = null!;
            ApplicationUser omarUser = null!;

            if (!await userManager.Users.AnyAsync())
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@novelhub.com",
                    Email = "admin@novelhub.com",
                    EmailConfirmed = true,
                    FullName = "أحمد القاضي - إدارة المنصة",
                    RegisteredAt = DateTime.UtcNow.AddMonths(-6)
                };
                await userManager.CreateAsync(adminUser, "Admin@123456");
                await userManager.AddToRoleAsync(adminUser, "Admin");

                readerUser = new ApplicationUser
                {
                    UserName = "reader@novelhub.com",
                    Email = "reader@novelhub.com",
                    EmailConfirmed = true,
                    FullName = "محمود حسن - القارئ الذهبي",
                    RegisteredAt = DateTime.UtcNow.AddMonths(-3)
                };
                await userManager.CreateAsync(readerUser, "User@123456");
                await userManager.AddToRoleAsync(readerUser, "Reader");

                saraUser = new ApplicationUser
                {
                    UserName = "sara@novelhub.com",
                    Email = "sara@novelhub.com",
                    EmailConfirmed = true,
                    FullName = "سارة أحمد",
                    RegisteredAt = DateTime.UtcNow.AddMonths(-2)
                };
                await userManager.CreateAsync(saraUser, "User@123456");
                await userManager.AddToRoleAsync(saraUser, "Reader");

                omarUser = new ApplicationUser
                {
                    UserName = "omar@novelhub.com",
                    Email = "omar@novelhub.com",
                    EmailConfirmed = true,
                    FullName = "عمر الفاروق",
                    RegisteredAt = DateTime.UtcNow.AddMonths(-1)
                };
                await userManager.CreateAsync(omarUser, "User@123456");
                await userManager.AddToRoleAsync(omarUser, "Reader");
            }
            else
            {
                adminUser = (await userManager.FindByEmailAsync("admin@novelhub.com"))!;
                readerUser = (await userManager.FindByEmailAsync("reader@novelhub.com"))!;
                saraUser = (await userManager.FindByEmailAsync("sara@novelhub.com"))!;
                omarUser = (await userManager.FindByEmailAsync("omar@novelhub.com"))!;
            }

            // 3. Seed Categories & Novels if none exist
            if (!await context.Categories.AnyAsync())
            {
                var catFantasy = new Category { Name = "خيال وعوالم موازية", Description = "رحلات عبر الأبعاد والسحر والواقع البديل", IconClass = "fas fa-dragon" };
                var catMystery = new Category { Name = "غموض وجريمة", Description = "تحقيقات، أسرار غامضة وحبكة ملحمية", IconClass = "fas fa-user-secret" };
                var catRomance = new Category { Name = "دراما ورومانسية", Description = "قصص المشاعر الإنسانية والصراعات العاطفية", IconClass = "fas fa-heart" };
                var catHistory = new Category { Name = "تاريخ وأساطير", Description = "الملحمة التاريخية والبطولات القديمة", IconClass = "fas fa-monument" };

                await context.Categories.AddRangeAsync(catFantasy, catMystery, catRomance, catHistory);
                await context.SaveChangesAsync();

                // Novels
                var novel1 = new Novel
                {
                    Title = "سر المخطوطة الكونية",
                    Author = "د. طارق السعدني",
                    Description = "في قلب مكتبة قديمة بالقاهرة، يعثر الباحث شاب على مخطوطة نادرة تكشف عن بوابة زمنية تحرسها ظلال غامضة. رحلة بين الحاضر والماضي لحماية أسرار الكون من أيدي منظمة سريّة.",
                    CoverImageUrl = "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?auto=format&fit=crop&w=600&q=80",
                    CategoryId = catFantasy.Id,
                    IsPublished = true,
                    IsFeatured = true,
                    Rating = 4.9,
                    ViewsCount = 14500,
                    CreatedAt = DateTime.UtcNow.AddDays(-60)
                };

                var novel2 = new Novel
                {
                    Title = "ظل في أزقة القاهرة",
                    Author = "ميرنا الشريف",
                    Description = "جريمة غامضة تتحدّى المحقق الكسار في ليالي الشتاء الباردة. خيوط الشبهات تلتف حول شخصيات نافذة في المجتمع، وكل فصل يكشف سرًا جديدًا أشد خطورة.",
                    CoverImageUrl = "https://images.unsplash.com/photo-1509021436468-d5103e390c0f?auto=format&fit=crop&w=600&q=80",
                    CategoryId = catMystery.Id,
                    IsPublished = true,
                    IsFeatured = true,
                    Rating = 4.8,
                    ViewsCount = 9800,
                    CreatedAt = DateTime.UtcNow.AddDays(-45)
                };

                var novel3 = new Novel
                {
                    Title = "عشق بين طلل الزمان",
                    Author = "يوسف العلي",
                    Description = "رواية رومانسية درامية تجمع بين قلبين فرقتهما الأحداث والطبقات الاجتماعية، ليجمعهما القدر مجددًا بعد سنوات من الغياب والمخاطر.",
                    CoverImageUrl = "https://images.unsplash.com/photo-1512820790803-83ca734da794?auto=format&fit=crop&w=600&q=80",
                    CategoryId = catRomance.Id,
                    IsPublished = true,
                    IsFeatured = true,
                    Rating = 4.7,
                    ViewsCount = 12300,
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                };

                var novel4 = new Novel
                {
                    Title = "حارس عرش بابل",
                    Author = "خالد الفارس",
                    Description = "ملحمة تاريخية مشوقة تدور أحداثها في العصور القديمة حول فارس يقسم على حماية المدينة العظيمة ضد الغزاة وأسوار الأسرار السياسية.",
                    CoverImageUrl = "https://images.unsplash.com/photo-1461360370896-922624d12aa1?auto=format&fit=crop&w=600&q=80",
                    CategoryId = catHistory.Id,
                    IsPublished = true,
                    IsFeatured = false,
                    Rating = 4.9,
                    ViewsCount = 8200,
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                };

                var novel5 = new Novel
                {
                    Title = "شفرة الأندلس المفقودة",
                    Author = "د. طارق السعدني",
                    Description = "البحث عن كنوز الأندلس المخبأة في رسائل رمزية بين المخطوطات القديمة. مغامرة مليئة بالتشويق والألغاز العقلية الذكية.",
                    CoverImageUrl = "https://images.unsplash.com/photo-1476275466078-4007374efbbe?auto=format&fit=crop&w=600&q=80",
                    CategoryId = catMystery.Id,
                    IsPublished = true,
                    IsFeatured = false,
                    Rating = 4.6,
                    ViewsCount = 6400,
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                };

                await context.Novels.AddRangeAsync(novel1, novel2, novel3, novel4, novel5);
                await context.SaveChangesAsync();

                // 4. Seed Chapters for Novels
                var novelsList = new[] { novel1, novel2, novel3, novel4, novel5 };
                var createdChapters = new List<Chapter>();

                foreach (var novel in novelsList)
                {
                    for (int chNum = 1; chNum <= 6; chNum++)
                    {
                        bool isFree = chNum <= 2; // Chapter 1 & 2 are Free, 3+ Paid
                        decimal price = isFree ? 0.00m : (15.00m + (chNum * 5));

                        string chapterTitle = chNum switch
                        {
                            1 => "الفصل الأول: البداية والغضب الخفي",
                            2 => "الفصل الثاني: المخطوطة الغامضة",
                            3 => "الفصل الثالث: بوابة الظلال والهمس",
                            4 => "الفصل الرابع: المواجهة الحتمية",
                            5 => "الفصل الخامس: لغز الغرفة المغلقة",
                            _ => $"الفصل السادس: الحقيقة الكبرى"
                        };

                        string textContent = $@"
<p class='lead font-weight-bold'>كانت الساعة تشير إلى منتصف الليل عندما بدأ كل شيء. خيم الهدوء الساكن على المكان، ولم يكن يُسمع سوى صوت أوراق الأشجار التي تحركها رياح الشتاء الباردة.</p>

<p>جلس الأستاذ يراقب الشاشة في صمت ذهول، فالرموز القديمة المحفورة على حافة المخطوطة لم تكن مجرد نقوش عادية، بل كانت شفرة حسابية معقدة صممت بعناية لتخفي أسرارًا لم تشهدها البشرية من قبل. مسح جبينه بيده المرتجفة وهو يستحضر كلمات والده الراحل: 'إذا وجدت المفتاح، فلا تفتحه وحدك'.</p>

<blockquote class='my-4 p-3 bg-light border-right border-primary rounded'>
""الأسرار العظيمة لا تبحث عن قُرّائها، بل القُرّاء هم من يتعثرون بها في لحظات القدر الحرج.""
</blockquote>

<p>تخطى الخطوة الأولى في الممر المظلم المؤدي إلى القبو السري. كان الهواء دافئًا بشكل غير عادي، ورائحة العود القديم تملأ الفضاء. خطوة تلو الأخرى، أحس بأنه يبتعد عن العالم الخارجي ويدخل في حقبة زمانية أخرى تمامًا. هناك في زاوية الغرفة، لمح الصندوق الخشبي المطعم بالفضة والياقوت.</p>

<p>اقترب بهدوء، وأخرج المفتاح البرونزي من جيبه. وضعه في القفل، ودار المفتاح بدورته الأولى محققًا صوتاً ميكانيكياً خفيفاً... وفي تلك اللحظة بالذات، انقطعت الأضواء فجأة وارتفع صوت خطى سريعة خلفه في الرداء المعتم!</p>

<p>توقف نبضه للحظة، وحبس أنفاسه وهو يستعد لمواجهة المجهول الحتمي...</p>";

                        var chapter = new Chapter
                        {
                            NovelId = novel.Id,
                            ChapterNumber = chNum,
                            Title = $"{chapterTitle}",
                            Content = textContent.Trim(),
                            Price = price,
                            IsFree = isFree,
                            IsPublished = true,
                            CreatedAt = DateTime.UtcNow.AddDays(-60 + chNum)
                        };

                        createdChapters.Add(chapter);
                    }
                }

                await context.Chapters.AddRangeAsync(createdChapters);
                await context.SaveChangesAsync();

                // 5. Seed Historical Purchases & Orders for demo accounts
                var paidChaptersToPurchase = createdChapters.Where(c => !c.IsFree).Take(12).ToList();
                int orderCounter = 1000;

                foreach (var ch in paidChaptersToPurchase)
                {
                    // Alternate between readerUser, saraUser, omarUser
                    var buyer = (orderCounter % 3) switch
                    {
                        0 => readerUser,
                        1 => saraUser,
                        _ => omarUser
                    };

                    orderCounter++;
                    var orderNumber = $"ORD-{DateTime.UtcNow.Year}-{orderCounter}";

                    var order = new Order
                    {
                        OrderNumber = orderNumber,
                        UserId = buyer.Id,
                        ChapterId = ch.Id,
                        Amount = ch.Price,
                        Status = OrderStatus.Completed,
                        CreatedAt = DateTime.UtcNow.AddDays(-15 + (orderCounter % 10))
                    };
                    await context.Orders.AddAsync(order);
                    await context.SaveChangesAsync();

                    var payment = new Payment
                    {
                        OrderId = order.Id,
                        Provider = (orderCounter % 2 == 0) ? "Paymob / الفيزا المصرفية" : "فوري - Fawry Express",
                        TransactionReference = $"TXN-{Guid.NewGuid().ToString("N")[..10].ToUpper()}",
                        PaymentMethod = (orderCounter % 2 == 0) ? "بطاقة ائتمان" : "محفظة إلكترونية",
                        Status = "Success",
                        Amount = ch.Price,
                        PaidAt = order.CreatedAt.AddSeconds(45)
                    };
                    await context.Payments.AddAsync(payment);

                    var purchase = new Purchase
                    {
                        UserId = buyer.Id,
                        ChapterId = ch.Id,
                        PricePaid = ch.Price,
                        PurchasedAt = order.CreatedAt.AddSeconds(45)
                    };
                    await context.Purchases.AddAsync(purchase);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
