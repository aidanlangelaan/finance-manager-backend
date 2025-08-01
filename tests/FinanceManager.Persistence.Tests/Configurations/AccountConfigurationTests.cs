using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using FinanceManager.Persistence.Tests.Common;

namespace FinanceManager.Persistence.Tests.Configurations;

public class AccountConfigurationTests : PersistenceTestBase
{
    [Fact]
    public void Account_Properties_AreConfiguredCorrectly()
    {
        using (var context = DbContext)
        {
            var accountEntityType = context.Model.FindEntityType(typeof(Account));

            accountEntityType.ShouldNotBeNull();

            // Test Name property
            var nameProperty = accountEntityType.FindProperty(nameof(Account.Name));
            nameProperty.ShouldNotBeNull();
            nameProperty.IsNullable.ShouldBeFalse();

            // Test Type property
            var typeProperty = accountEntityType.FindProperty(nameof(Account.Type));
            typeProperty.ShouldNotBeNull();
            typeProperty.IsNullable.ShouldBeFalse();

            // Test Description property
            var descriptionProperty = accountEntityType.FindProperty(nameof(Account.Description));
            descriptionProperty.ShouldNotBeNull();
            descriptionProperty.IsNullable.ShouldBeTrue();

            // Test CurrentBalance property
            var currentBalanceProperty = accountEntityType.FindProperty(nameof(Account.CurrentBalance));
            currentBalanceProperty.ShouldNotBeNull();
            currentBalanceProperty.IsNullable.ShouldBeFalse();

            // Test IncludedInNetWorth property
            var includedInNetWorthProperty = accountEntityType.FindProperty(nameof(Account.IncludedInNetWorth));
            includedInNetWorthProperty.ShouldNotBeNull();
            includedInNetWorthProperty.IsNullable.ShouldBeFalse();

            // Test CanTransferFrom property
            var canTransferFromProperty = accountEntityType.FindProperty(nameof(Account.CanTransferFrom));
            canTransferFromProperty.ShouldNotBeNull();
            canTransferFromProperty.IsNullable.ShouldBeFalse();

            // Test CanTransferTo property
            var canTransferToProperty = accountEntityType.FindProperty(nameof(Account.CanTransferTo));
            canTransferToProperty.ShouldNotBeNull();
            canTransferToProperty.IsNullable.ShouldBeFalse();
        }
    }

    [Fact]
    public void Account_Relationships_AreConfiguredCorrectly()
    {
        using (var context = DbContext)
        {
            var accountEntityType = context.Model.FindEntityType(typeof(Account));

            accountEntityType.ShouldNotBeNull();

            // Test CreatedBy relationship
            var createdByNavigation = accountEntityType.FindNavigation(nameof(Account.CreatedBy));
            createdByNavigation.ShouldNotBeNull();
            createdByNavigation.IsCollection.ShouldBeFalse();
            createdByNavigation.ForeignKey.Properties.ShouldContain(p => p.Name == nameof(Account.CreatedById));
            createdByNavigation.ForeignKey.DeleteBehavior.ShouldBe(DeleteBehavior.Restrict);

            // Test SourceTransactions relationship
            var sourceTransactionsNavigation = accountEntityType.FindNavigation(nameof(Account.SourceTransactions));
            sourceTransactionsNavigation.ShouldNotBeNull();
            sourceTransactionsNavigation.IsCollection.ShouldBeTrue();
            sourceTransactionsNavigation.ForeignKey.Properties.ShouldContain(p => p.Name == nameof(Transaction.SourceAccountId));
            sourceTransactionsNavigation.ForeignKey.DeleteBehavior.ShouldBe(DeleteBehavior.Cascade);

            // Test DestinationTransactions relationship
            var destinationTransactionsNavigation = accountEntityType.FindNavigation(nameof(Account.DestinationTransactions));
            destinationTransactionsNavigation.ShouldNotBeNull();
            destinationTransactionsNavigation.IsCollection.ShouldBeTrue();
            destinationTransactionsNavigation.ForeignKey.Properties.ShouldContain(p => p.Name == nameof(Transaction.DestinationAccountId));
            destinationTransactionsNavigation.ForeignKey.DeleteBehavior.ShouldBe(DeleteBehavior.Cascade);
        }
    }
}

